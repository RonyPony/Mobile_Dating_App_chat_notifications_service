using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2.Requests;
using NotificationCenter.Core.Contracts;
using NotificationCenter.Core.Domain;
using NotificationCenter.Core.Models;
using NotificationCenter.Core.Validations;

namespace NotificationCenter.Core.Managers
{
    /// <summary>
    /// Represents a manager that's responsible of <see cref="TokenUser"/>s and <see cref="Device"/>s.
    /// </summary>
    public class TokenManager
    {
        private readonly ITokenUserStoreService _tokenUserStoreServce;
        private readonly IDeviceStoreService _deviceStoreService;
        private readonly IAppService _appService;

        /// <summary>
        /// Builds a new instance of the notification manager
        /// </summary>
        /// <param name="tokenUserStoreServce">Receives an implementation of <see cref="ITokenUserStoreService"/></param>
        /// <param name="deviceStoreService">An implementation of <see cref="IDeviceStoreService"/></param>
        public TokenManager(ITokenUserStoreService tokenUserStoreServce, IDeviceStoreService deviceStoreService, IAppService appService)
        {
            _tokenUserStoreServce = tokenUserStoreServce;
            _deviceStoreService = deviceStoreService;
            _appService = appService;
        }

        /// <summary>
        /// Registers a new token to the database while creating the related user object, or updates an existing device if device is already reistered.
        /// </summary>
        /// <param name="tokenRequest">A <see cref="TokenRequest"/> item</param>
        /// <returns></returns>
        public async Task<IOperationResult<Device>> EstablishUserToken(UserTokenRequest tokenRequest)
        {
            try
            {
                TokenUser tokenUser = await GetTokenUser(tokenRequest);

                var validationResults = ValidateTokenUser(tokenUser);
                if (validationResults != null)
                {
                    return BasicOperationResult<Device>.Fail(validationResults.First());
                }

                Device device = await _deviceStoreService.GetDeviceByIdentifier(tokenRequest.DeviceIdentifier, tokenRequest.AppName);

                if (device == null || (device != null && device.TokenUser != tokenUser))
                {
                    if (device != null && device.TokenUser != tokenUser)
                        await RemoveDeviceByIdentifier(tokenRequest.DeviceIdentifier, tokenRequest.AppName);

                    IOperationResult<Device> registerResult = await RegisterToken(tokenRequest, tokenUser);
                    if (!registerResult.Success)
                        return registerResult;

                    device = registerResult.Data;
                }
                else
                {
                    IOperationResult<Device> updateResult = await UpdateToken(tokenRequest);
                    if (!updateResult.Success)
                        return updateResult;

                    device = updateResult.Data;
                }

                validationResults = ValidateDevice(device);
                if (validationResults != null)
                {
                    return BasicOperationResult<Device>.Fail(validationResults.First());
                }

                return BasicOperationResult<Device>.Ok(device);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<Device>.Fail(ex.Message, ex);
            }
        }

        /// <summary>
        /// Removes an Device from the Database
        /// </summary>
        /// <param name="fcmToken">A <see cref="string"/> value</param>
        /// <returns>A <see cref="bool"/> value</returns>
        public async Task<IOperationResult<Device>> RemoveDeviceByIdentifier(string identifier, string appName)
        {
            Device device = await _deviceStoreService.GetDeviceByIdentifier(identifier, appName);

            if (device == null)
            {
                return BasicOperationResult<Device>.Fail("No device was found for specified fcmToken");
            }

            await _deviceStoreService.RemoveDevice(device);

            return BasicOperationResult<Device>.Ok(device);
        }

        /// <summary>
        /// Gets all of the device types
        /// </summary>
        public Dictionary<int, string> GetDeviceTypes => _deviceStoreService.GetDeviceTypes();

        #region Private methods
        /// <summary>
        /// Registers a new token to the database while creating the related user object
        /// </summary>
        /// <param name="tokenRequest">A <see cref="TokenRequest"/> item</param>
        /// <returns></returns>
        private async Task<IOperationResult<Device>> RegisterToken(UserTokenRequest tokenRequest, TokenUser tokenUser)
        {
            try
            {
                Device device = await _deviceStoreService.GetDeviceByIdentifier(tokenRequest.DeviceIdentifier, tokenRequest.AppName);

                if (device == null)
                {
                    App app = await _appService.GetAppByName(tokenRequest.AppName);

                    if (app == null)
                    {
                        return BasicOperationResult<Device>.Fail("No app with the given name was found");
                    }

                    device = await _deviceStoreService.AddNewDevice(new Device
                    {
                        FcmToken = tokenRequest.DeviceToken,
                        ApnsToken = tokenRequest.ApnsToken,
                        DeviceIdentifier = tokenRequest.DeviceIdentifier,
                        AppId = app.Id,
                        TokenUserId = tokenUser.Id,
                        Type = tokenRequest.DeviceType
                    });
                }

                var validationResults = ValidateDevice(device);
                if (validationResults != null)
                {
                    return BasicOperationResult<Device>.Fail(validationResults.First());
                }

                return BasicOperationResult<Device>.Ok(device);
            }
            catch (Exception ex)
            {
                return BasicOperationResult<Device>.Fail(ex.Message, ex);
            }
        }

        /// <summary>
        /// Updates the tokens of an existing device.
        /// </summary>
        /// <param name="updateRequest">A <see cref="UserTokenRequest"/> object with the information for the update.</param>
        /// <returns></returns>
        private async Task<IOperationResult<Device>> UpdateToken(UserTokenRequest updateRequest)
        {
            var device = await _deviceStoreService.UpdateDeviceTokens(updateRequest);

            if (device == null)
            {
                return BasicOperationResult<Device>.Fail("Unable to update the token for the specified device");
            }

            return BasicOperationResult<Device>.Ok(device);
        }

        private async Task<TokenUser> GetTokenUser(UserTokenRequest tokenRequest) =>
                   await _tokenUserStoreServce.GetTokenUserByUserId(tokenRequest.UserId) ??
                   await _tokenUserStoreServce.AddTokenUser(new TokenUser
                   {
                       Name = tokenRequest.Name,
                       LastName = tokenRequest.LastName,
                       UserId = tokenRequest.UserId
                   });

        private static List<string> ValidateTokenUser(TokenUser tokenUser)
        {
            var validator = new TokenUserValidator();
            var validations = validator.Validate(tokenUser);
            if (validations.IsValid)
            {
                return null;
            }
            return validations.Errors.Select(x => x.ErrorMessage).ToList();
        }

        private static List<string> ValidateDevice(Device device)
        {
            var validator = new DeviceValidator();
            var validations = validator.Validate(device);
            if (validations.IsValid)
            {
                return null;
            }
            return validations.Errors.Select(x => x.ErrorMessage).ToList();
        }
        #endregion
    }
}

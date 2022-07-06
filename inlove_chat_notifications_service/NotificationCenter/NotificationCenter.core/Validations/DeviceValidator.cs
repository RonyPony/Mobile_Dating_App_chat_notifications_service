using FluentValidation;
using NotificationCenter.Core.Domain;

namespace NotificationCenter.Core.Validations
{
    /// <summary>
    /// Represents a validator for <see cref="Device"/>
    /// </summary>
    public class DeviceValidator : AbstractValidator<Device>
    {
        /// <summary>
        /// Builds a new instance of <see cref="DeviceValidator"/>
        /// </summary>
        public DeviceValidator()
        {
            RuleFor(x => x.FcmToken).NotEmpty().WithMessage("FcmToken.Required");
            RuleFor(x => x.TokenUserId).NotEmpty().WithMessage("TokenUserId.Required");
            RuleFor(x => x.Type).NotNull().WithMessage("Type.Required");
        }
    }
}

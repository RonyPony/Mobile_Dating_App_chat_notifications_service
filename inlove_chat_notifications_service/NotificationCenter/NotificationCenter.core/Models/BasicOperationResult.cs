using NotificationCenter.Core.Contracts;
using System;
using System.Text.Json.Serialization;

namespace NotificationCenter.Core.Models
{
    /// <summary>
    /// Represents an implementation of IOperationResult
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BasicOperationResult<T> : IOperationResult<T>
    {
        ///<inheritdoc cref="IOperationResult{T}.Data"/>
        public T Data { get; }

        ///<inheritdoc cref="IOperationResult{T}.Success"/>
        public bool Success { get; }

        ///<inheritdoc cref="IOperationResult{T}.Message"/>
        public string Message { get; }

        ///<inheritdoc cref="IOperationResult{T}.ErrorDetail"/>
        [JsonIgnore]
        public Exception ErrorDetail { get; }

        internal BasicOperationResult(T data, bool success, string message, Exception errorDetail = null)
        {
            this.Data = data;
            this.Success = success;
            this.Message = message;
            this.ErrorDetail = errorDetail;
        }

        /// <summary>
        /// In case of succesfull response, but no data returned
        /// </summary>
        /// <returns></returns>
        public static IOperationResult<T> Ok() => new BasicOperationResult<T>(default, true, null);

        /// <summary>
        /// In Case of succesfull response with associated data
        /// </summary>
        /// <param name="data">Represents the input data</param>
        /// <returns></returns>
        public static IOperationResult<T> Ok(T data) => new BasicOperationResult<T>(data, true, null);

        /// <summary>
        /// In case of failed response
        /// </summary>
        /// <param name="message">error message associated with failure</param>
        /// <returns></returns>
        public static IOperationResult<T> Fail(string message) => new BasicOperationResult<T>(default, false, message);

        /// <summary>
        /// In case of failed response with data response
        /// </summary>
        /// <param name="data">Data returned with given failure</param>
        /// <param name="message">Error message associated with failure</param>
        /// <returns></returns>
        public static IOperationResult<T> Fail(T data, string message) => new BasicOperationResult<T>(data, false, message);

        /// <summary>
        /// In case of failed response with an exception message
        /// </summary>
        /// <param name="message">error message associated with failure</param>
        /// <returns></returns>
        public static IOperationResult<T> Fail(string message, Exception errorDetail) => new BasicOperationResult<T>(default, false, message, errorDetail);
    }
}
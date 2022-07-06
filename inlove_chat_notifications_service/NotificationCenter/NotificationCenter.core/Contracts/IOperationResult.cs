using System;
using System.Text.Json.Serialization;

namespace NotificationCenter.Core.Contracts
{
    /// <summary>
    /// Represents a response structure
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IOperationResult<T>
    {
        /// <summary>
        /// The data returned if success
        /// </summary>
        T Data { get; }

        /// <summary>
        /// Tells whether it was a success or not
        /// </summary>
        bool Success { get; }

        /// <summary>
        /// Error message in case of errors performing action
        /// </summary>
        string Message { get; }

        /// <summary>
        /// The detailof an exception in case of error.
        /// </summary>
        [JsonIgnore]
        Exception ErrorDetail { get; }

        /// <summary>
        /// Indicattes whether this result contains an error or not.
        /// </summary>
        bool ContainsError { get => ErrorDetail != null; }
    }
}
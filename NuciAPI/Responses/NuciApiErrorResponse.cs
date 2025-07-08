using System;
using System.Text.Json.Serialization;

namespace NuciAPI.Responses
{
    /// <summary>
    /// Represents a response indicating an error occurred during processing.
    /// </summary>
    public sealed class NuciApiErrorResponse : NuciApiResponse
    {
        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public override bool IsSuccessful => false;

        /// <summary>
        /// Default constructor for ErrorResponse.
        /// </summary>
        public NuciApiErrorResponse() : base(NuciApiResponseMessages.ErrorMessages.Default) { }

        /// <summary>
        /// Initializes a new instance of the ErrorResponse class with a specific error message.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        public NuciApiErrorResponse(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ErrorResponse class with an exception.
        /// </summary>
        /// <param name="exception">The exception that occurred, which will be used to set the error message.</param>
        public NuciApiErrorResponse(Exception exception) : base(exception.Message) { }

        /// <summary>
        /// Creates a new ErrorResponse instance from a specific error message.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        /// <returns>A new instance of ErrorResponse with the specified message.</returns>
        public static NuciApiErrorResponse FromMessage(string message) => new(message);

        /// <summary>
        /// Creates a new ErrorResponse instance from an exception.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A new instance of ErrorResponse with the exception's message.</returns>
        public static NuciApiErrorResponse FromException(Exception exception) => new(exception);

        /// <summary>
        /// Provides a default ErrorResponse instance with a standard error message.
        /// </summary>
        public static NuciApiErrorResponse Default => new(NuciApiResponseMessages.ErrorMessages.Default);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating an invalid request.
        /// </summary>
        public static NuciApiErrorResponse InvalidRequest => new(NuciApiResponseMessages.ErrorMessages.InvalidRequest);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the requested resource was not found.
        /// </summary>
        public static NuciApiErrorResponse NotFound => new(NuciApiResponseMessages.ErrorMessages.NotFound);
    }
}

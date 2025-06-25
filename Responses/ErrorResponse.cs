using System;

namespace NuciAPI.Responses
{
    /// <summary>
    /// Represents a response indicating an error occurred during processing.
    /// </summary>
    public sealed class ErrorResponse : Response
    {
        /// <summary>
        /// Indicates whether the response was successful.
        /// </summary>
        public override bool Success => false;

        /// <summary>
        /// Default constructor for ErrorResponse.
        /// </summary>
        public ErrorResponse() : base(ErrorResponseMessages.Default) { }

        /// <summary>
        /// Initializes a new instance of the ErrorResponse class with a specific error message.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        public ErrorResponse(string message) : base(message) { }

        /// <summary>
        /// Initializes a new instance of the ErrorResponse class with an exception.
        /// </summary>
        /// <param name="exception">The exception that occurred, which will be used to set the error message.</param>
        public ErrorResponse(Exception exception) : base(exception.Message) { }

        /// <summary>
        /// Creates a new ErrorResponse instance from a specific error message.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        /// <returns>A new instance of ErrorResponse with the specified message.</returns>
        public static ErrorResponse FromMessage(string message) => new(message);

        /// <summary>
        /// Creates a new ErrorResponse instance from an exception.
        /// </summary>
        /// <param name="exception">The exception that occurred.</param>
        /// <returns>A new instance of ErrorResponse with the exception's message.</returns>
        public static ErrorResponse FromException(Exception exception) => new(exception);

        /// <summary>
        /// Provides a default ErrorResponse instance with a standard error message.
        /// </summary>
        public static ErrorResponse Default => new(ErrorResponseMessages.Default);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating an invalid request.
        /// </summary>
        public static ErrorResponse InvalidRequest => new(ErrorResponseMessages.InvalidRequest);
    }
}

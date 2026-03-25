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
        public NuciApiErrorResponse() : base(
            NuciApiResponseMessages.ErrorMessages.Default,
            NuciApiResponseCodes.ErrorCodes.Default) { }

        /// <summary>
        /// Initializes a new instance of the ErrorResponse class with a specific error message and code.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        /// <param name="code">The error code to include in the response.</param>
        public NuciApiErrorResponse(string message, string code) : base(message, code) { }

        /// <summary>
        /// Creates a new ErrorResponse instance from a specific error message.
        /// </summary>
        /// <param name="message">The error message to include in the response.</param>
        /// <returns>A new instance of ErrorResponse with the specified message.</returns>
        public static NuciApiErrorResponse FromMessage(string message) => new(
            message,
            NuciApiResponseCodes.ErrorCodes.Default);

        /// <summary>
        /// Provides a default ErrorResponse instance with a standard error message.
        /// </summary>
        public static NuciApiErrorResponse Default => new(
            NuciApiResponseMessages.ErrorMessages.Default,
            NuciApiResponseCodes.ErrorCodes.Default);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the requested resource already exists.
        /// </summary>
        public static NuciApiErrorResponse AlreadyExists => new(
            NuciApiResponseMessages.ErrorMessages.AlreadyExists,
            NuciApiResponseCodes.ErrorCodes.AlreadyExists);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the request has already been processed.
        /// </summary>
        public static NuciApiErrorResponse AlreadyProcessed => new(
            NuciApiResponseMessages.ErrorMessages.AlreadyProcessed,
            NuciApiResponseCodes.ErrorCodes.AlreadyProcessed);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the request failed due to authentication issues.
        /// </summary>
        public static NuciApiErrorResponse AuthenticationFailure => new(
            NuciApiResponseMessages.ErrorMessages.AuthenticationFailure,
            NuciApiResponseCodes.ErrorCodes.AuthenticationFailure);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the request failed due to a bad request, such as invalid input or missing parameters.
        /// </summary>
        public static NuciApiErrorResponse BadRequest => new(
            NuciApiResponseMessages.ErrorMessages.BadRequest,
            NuciApiResponseCodes.ErrorCodes.BadRequest);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the client closed the request.
        /// </summary>
        public static NuciApiErrorResponse ClientClosedTheRequest => new(
            NuciApiResponseMessages.ErrorMessages.ClientClosedTheRequest,
            NuciApiResponseCodes.ErrorCodes.ClientClosedTheRequest);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that an internal server error occurred.
        /// </summary>
        public static NuciApiErrorResponse InternalServerError => new(
            NuciApiResponseMessages.ErrorMessages.InternalServerError,
            NuciApiResponseCodes.ErrorCodes.InternalServerError);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating an invalid request.
        /// </summary>
        public static NuciApiErrorResponse InvalidRequest => new(
            NuciApiResponseMessages.ErrorMessages.InvalidRequest,
            NuciApiResponseCodes.ErrorCodes.InvalidRequest);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the requested resource was not found.
        /// </summary>
        public static NuciApiErrorResponse NotFound => new(
            NuciApiResponseMessages.ErrorMessages.NotFound,
            NuciApiResponseCodes.ErrorCodes.NotFound);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the service dependency is unavailable.
        /// </summary>
        public static NuciApiErrorResponse ServiceDependencyUnavailable => new(
            NuciApiResponseMessages.ErrorMessages.ServiceDependencyUnavailable,
            NuciApiResponseCodes.ErrorCodes.ServiceDependencyUnavailable);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the request timed out.
        /// </summary>
        public static NuciApiErrorResponse Timeout => new(
            NuciApiResponseMessages.ErrorMessages.Timeout,
            NuciApiResponseCodes.ErrorCodes.Timeout);

        /// <summary>
        /// Provides a default ErrorResponse instance indicating that the request was forbidden.
        /// </summary>
        public static NuciApiErrorResponse Unauthorised => new(
            NuciApiResponseMessages.ErrorMessages.Unauthorised,
            NuciApiResponseCodes.ErrorCodes.Unauthorised);

    }
}

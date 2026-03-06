namespace NuciAPI.Responses
{
    /// <summary>
    /// Contains standard response messages used across the API.
    /// </summary>
    public static class NuciApiResponseMessages
    {
        /// <summary>
        /// Contains standard success response messages used across the API.
        /// </summary>
        public static class SuccessMessages
        {
            /// <summary>
            /// Default success message used when no specific message is provided.
            /// </summary>
            public const string Default = "Operation completed successfully.";
        }

        /// <summary>
        /// Contains standard error response messages used across the API.
        /// </summary>
        public static class ErrorMessages
        {
            /// <summary>
            /// Default error message used when no specific error is provided.
            /// </summary>
            public const string Default = "An error occurred while processing your request.";

            /// <summary>
            /// Error message indicating that the requested resource already exists.
            /// </summary>
            public const string AlreadyExists = "The requested resource already exists.";

            /// <summary>
            /// Error message indicating that the request failed due to authentication issues.
            /// </summary>
            public const string AuthenticationFailure = "The authentication has failed.";

            /// <summary>
            /// Error message indicating that the client closed the request.
            /// </summary>
            public const string ClientClosedTheRequest = "The client has closed the request.";

            /// <summary>
            /// Error message indicating that the request was forbidden.
            /// </summary>
            public const string Forbidden = "You do not have the required permission to access this resource.";

            /// <summary>
            /// Error message indicating that the request is invalid.
            /// </summary>
            public const string InvalidRequest = "The request is invalid.";

            /// <summary>
            /// Error message indicating that the requested resource was not found.
            /// </summary>
            public const string NotFound = "The requested resource was not found.";

            /// <summary>
            /// Error message indicating that the service dependency is unavailable.
            /// </summary>
            public const string ServiceDependencyUnavailable = "A service dependency is currently unavailable.";

            /// <summary>
            /// Error message indicating that the request timed out.
            /// </summary>
            public const string Timeout = "The request has timed out.";
        }
    }
}

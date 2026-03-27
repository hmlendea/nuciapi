namespace NuciAPI.Responses
{
    /// <summary>
    /// Contains standard response codes used across the API.
    /// </summary>
    public static class NuciApiResponseCodes
    {
        /// <summary>
        /// Contains standard success response codes used across the API.
        /// </summary>
        public static class SuccessCodes
        {
            /// <summary>
            /// Default success code used when no specific code is provided.
            /// </summary>
            public const string Default = "SUCCESS";

            /// <summary>
            /// Success code indicating that a new resource was successfully created.
            /// </summary>
            public const string Created = "SUCCESS_CREATED";

            /// <summary>
            /// Success code indicating that the operation completed successfully but no changes were made.
            /// </summary>
            public const string NoChanges = "SUCCESS_NO_CHANGE";
        }

        /// <summary>
        /// Contains standard error response codes used across the API.
        /// </summary>
        public static class ErrorCodes
        {
            /// <summary>
            /// Default error code used when no specific error is provided.
            /// </summary>
            public const string Default = "ERROR";

            /// <summary>
            /// Error code indicating that the requested resource already exists.
            /// </summary>
            public const string AlreadyExists = "ALREADY_EXISTS";

            /// <summary>
            /// Error code indicating that the request has already been processed.
            /// </summary>
            public const string AlreadyProcessed = "ALREADY_PROCESSED";

            /// <summary>
            /// Error code indicating that the request failed due to a bad request, such as invalid input or missing parameters.
            /// </summary>
            public const string BadRequest = "BAD_REQUEST";

            /// <summary>
            /// Error code indicating that the request failed due to authentication issues.
            /// </summary>
            public const string AuthenticationFailure = "AUTHENTICATION_FAILURE";

            /// <summary>
            /// Error code indicating that the client closed the request.
            /// </summary>
            public const string ClientClosedTheRequest = "CLIENT_CLOSED_THE_REQUEST";

            /// <summary>
            /// Error code indicating that an internal server error occurred.
            /// </summary>
            public const string InternalServerError = "INTERNAL_SERVER_ERROR";

            /// <summary>
            /// Error code indicating that the request is invalid.
            /// </summary>
            public const string InvalidRequest = "INVALID_REQUEST";

            /// <summary>
            /// Error code indicating that the requested resource was not found.
            /// </summary>
            public const string NotFound = "NOT_FOUND";

            /// <summary>
            /// Error code indicating that the requested functionality is not implemented.
            /// </summary>
            public const string NotImplemented = "NOT_IMPLEMENTED";

            /// <summary>
            /// Error code indicating that the service dependency is unavailable.
            /// </summary>
            public const string ServiceDependencyUnavailable = "SERVICE_DEPENDENCY_UNAVAILABLE";

            /// <summary>
            /// Error code indicating that the request timed out.
            /// </summary>
            public const string Timeout = "TIMEOUT";

            /// <summary>
            /// Error code indicating that the request was forbidden.
            /// </summary>
            public const string Unauthorised = "UNAUTHORISED";
        }
    }
}

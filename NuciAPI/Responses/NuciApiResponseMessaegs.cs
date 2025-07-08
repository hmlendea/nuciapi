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
            /// Error message indicating that the request is invalid.
            /// </summary>
            public const string InvalidRequest = "Invalid request.";
        }
    }
}

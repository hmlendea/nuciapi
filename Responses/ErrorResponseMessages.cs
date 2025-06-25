namespace NuciAPI.Responses
{
    /// <summary>
    /// Contains standard error response messages used across the API.
    /// </summary>
    public static class ErrorResponseMessages
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

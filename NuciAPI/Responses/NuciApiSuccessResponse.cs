using System.Text.Json.Serialization;

namespace NuciAPI.Responses
{
    /// <summary>
    /// Represents a successful response from the API.
    /// </summary>
    public class NuciApiSuccessResponse : NuciApiResponse
    {
        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public override bool IsSuccessful => true;

        /// <summary>
        /// Default constructor for SuccessResponse.
        /// </summary>
        public NuciApiSuccessResponse() : base(
            NuciApiResponseMessages.SuccessMessages.Default,
            NuciApiResponseCodes.SuccessCodes.Default) { }

        /// <summary>
        /// Initializes a new instance of the SuccessResponse class with a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        public NuciApiSuccessResponse(string message) : base(
            message,
            NuciApiResponseCodes.SuccessCodes.Default) { }

        /// <summary>
        /// Initializes a new instance of the SuccessResponse class with a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        /// <param name="code">The code to include in the response.</param>
        public NuciApiSuccessResponse(
            string message,
            string code)
            : base(message, code) { }

        /// <summary>
        /// Creates a new SuccessResponse instance from a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        /// <returns></returns>
        public static NuciApiSuccessResponse FromMessage(string message) => new(message);

        /// <summary>
        /// Creates a new SuccessResponse instance with a default success message.
        /// </summary>
        public static NuciApiSuccessResponse Default => new(
            NuciApiResponseMessages.SuccessMessages.Default,
            NuciApiResponseCodes.SuccessCodes.Default);

        /// <summary>
        /// Creates a new SuccessResponse instance with a message indicating that the operation completed successfully but no changes were made.
        /// </summary>
        public static NuciApiSuccessResponse NoChange => new(
            NuciApiResponseMessages.SuccessMessages.NoChange,
            NuciApiResponseCodes.SuccessCodes.NoChange);
    }
}

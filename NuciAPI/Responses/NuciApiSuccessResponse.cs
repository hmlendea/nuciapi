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
        /// Default constructor for NuciApiSuccessResponse.
        /// </summary>
        public NuciApiSuccessResponse() : base(
            NuciApiResponseMessages.SuccessMessages.Default,
            NuciApiResponseCodes.SuccessCodes.Default) { }

        /// <summary>
        /// Initializes a new instance of the NuciApiSuccessResponse class with a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        public NuciApiSuccessResponse(string message) : base(
            message,
            NuciApiResponseCodes.SuccessCodes.Default) { }

        /// <summary>
        /// Initializes a new instance of the NuciApiSuccessResponse class with a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        /// <param name="code">The code to include in the response.</param>
        public NuciApiSuccessResponse(
            string message,
            string code)
            : base(message, code) { }

        /// <summary>
        /// Creates a new NuciApiSuccessResponse instance from a specific message.
        /// </summary>
        /// <param name="message">The message to include in the response.</param>
        /// <returns></returns>
        public static NuciApiSuccessResponse FromMessage(string message) => new(message);

        /// <summary>
        /// Creates a new NuciApiSuccessResponse instance with a default success message.
        /// </summary>
        public static NuciApiSuccessResponse Default => new(
            NuciApiResponseMessages.SuccessMessages.Default,
            NuciApiResponseCodes.SuccessCodes.Default);

        /// <summary>
        /// Creates a new NuciApiSuccessResponse instance with a message indicating that a new resource was successfully created.
        /// </summary>
        public static NuciApiSuccessResponse Created => new(
            NuciApiResponseMessages.SuccessMessages.Created,
            NuciApiResponseCodes.SuccessCodes.Created);

        /// <summary>
        /// Creates a new NuciApiSuccessResponse instance with a message indicating that a resource was successfully deleted.
        /// </summary>
        public static NuciApiSuccessResponse Deleted => new(
            NuciApiResponseMessages.SuccessMessages.Deleted,
            NuciApiResponseCodes.SuccessCodes.Deleted);

        /// <summary>
        /// Creates a new NuciApiSuccessResponse instance with a message indicating that a resource was not updated.
        /// </summary>
        public static NuciApiSuccessResponse NotUpdated => new(
            NuciApiResponseMessages.SuccessMessages.NotUpdated,
            NuciApiResponseCodes.SuccessCodes.NotUpdated);

        /// <summary>
        /// Creates a new NuciApiSuccessResponse instance with a message indicating that a resource was successfully updated.
        /// </summary>
        public static NuciApiSuccessResponse Updated => new(
            NuciApiResponseMessages.SuccessMessages.Updated,
            NuciApiResponseCodes.SuccessCodes.Updated);
    }
}

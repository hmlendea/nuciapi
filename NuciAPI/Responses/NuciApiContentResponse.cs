using System.Text.Json.Serialization;

namespace NuciAPI.Responses
{
    /// <summary>
    /// Represents a successful API response with strongly typed payload content.
    /// </summary>
    /// <typeparam name="TContent">The response payload content type.</typeparam>
    public sealed class NuciApiContentResponse<TContent> : NuciApiResponse
        where TContent : NuciApiResponseContent
    {
        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public override bool IsSuccessful => true;

        /// <summary>
        /// The response payload content.
        /// </summary>
        [JsonPropertyName("content")]
        public TContent Content { get; set; }

        /// <summary>
        /// Initialises a successful response with payload content.
        /// </summary>
        /// <param name="content">The response payload content.</param>
        public NuciApiContentResponse(TContent content) : base(
            NuciApiResponseMessages.SuccessMessages.Default,
            NuciApiResponseCodes.SuccessCodes.Default)
        {
            Content = content;
        }

        /// <summary>
        /// Initialises a successful response with payload content and a specific message.
        /// </summary>
        /// <param name="content">The response payload content.</param>
        /// <param name="message">The message to include in the response.</param>
        public NuciApiContentResponse(
            TContent content,
            string message)
            : base(message, NuciApiResponseCodes.SuccessCodes.Default)
        {
            Content = content;
        }

        /// <summary>
        /// Initialises a successful response with payload content, a message, and a code.
        /// </summary>
        /// <param name="content">The response payload content.</param>
        /// <param name="message">The message to include in the response.</param>
        /// <param name="code">The code to include in the response.</param>
        [JsonConstructor]
        public NuciApiContentResponse(
            TContent content,
            string message,
            string code)
            : base(message, code)
        {
            Content = content;
        }
    }
}

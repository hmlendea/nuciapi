using System.Text.Json.Serialization;
using NuciSecurity.HMAC;

namespace NuciAPI.Responses
{
    /// <summary>
    /// Base class for all API responses.
    /// </summary>
    /// <param name="message">The message to include in the response.</param>
    public abstract class NuciApiResponse(string message)
    {
        /// <summary>
        /// Indicates whether the request was successful.
        /// </summary>
        [JsonPropertyName("success")]
        public abstract bool IsSuccessful { get; }

        /// <summary>
        /// The message included in the response, typically used to convey success or error information.
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; } = message;

        [JsonPropertyName("hmac")]
        [HmacIgnore]
        /// <summary>
        /// HMAC token for response validation.
        /// This property is used to verify the integrity and authenticity of the response.
        /// It should be generated using the server's secret key and the response data.
        /// </summary>
        public string HmacToken { get; set; }
    }
}

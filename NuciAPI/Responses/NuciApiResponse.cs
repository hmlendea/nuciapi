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
        [HmacOrder(9999999)]
        public abstract bool IsSuccessful { get; }

        /// <summary>
        /// The message included in the response, typically used to convey success or error information.
        /// </summary>
        [JsonPropertyName("message")]
        [HmacOrder(9999998)]
        public string Message { get; set; } = message;

        [JsonPropertyName("hmac")]
        [HmacIgnore]
        /// <summary>
        /// HMAC token for response validation.
        /// This property is used to verify the integrity and authenticity of the response.
        /// It should be generated using the server's secret key and the response data.
        /// </summary>
        public string HmacToken { get; set; }

        /// <summary>
        /// Signs the response with an HMAC token using the provided secret key.
        /// </summary>
        /// <param name="secretKey">The secret key used to generate the HMAC token.</param>
        public void SignHMAC(string secretKey)
            => HmacToken = HmacEncoder.GenerateToken(this, secretKey);

        /// <summary>
        /// Checks if the HMAC token is valid using the provided secret key.
        /// </summary>
        /// <param name="secretKey">The secret key used to validate the HMAC token.</param>
        /// <returns>True if the HMAC token is valid; otherwise, false.</returns>
        public bool HasValidHMAC(string secretKey)
            => HmacValidator.IsTokenValid(HmacToken, this, secretKey);

        /// <summary>
        /// Validates the HMAC token using the provided secret key.
        /// </summary>
        /// <param name="secretKey">The secret key used to validate the HMAC token.</param>
        public void ValidateHMAC(string secretKey)
            => HmacValidator.Validate(HmacToken, this, secretKey);
    }
}

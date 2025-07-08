using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using NuciSecurity.HMAC;

namespace NuciAPI.Requests
{
    /// <summary>
    /// Base class for all API requests.
    /// </summary>
    public abstract class NuciApiRequest()
    {
        [Required]
        [JsonPropertyName("hmac")]
        [HmacIgnore]
        /// <summary>
        /// HMAC token for request validation.
        /// This property is used to verify the integrity and authenticity of the request.
        /// It should be generated using the server's secret key and the request data.
        /// </summary>
        public string HmacToken { get; private set; }

        /// <summary>
        /// Signs the request with an HMAC token using the provided secret key.
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

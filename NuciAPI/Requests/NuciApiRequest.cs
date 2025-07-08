using System;
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

        public void SignHMAC(string secretKey)
        {
            if (string.IsNullOrEmpty(secretKey))
            {
                throw new ArgumentException("The secret key cannot be null or empty.", nameof(secretKey));
            }

            HmacToken = HmacEncoder.GenerateToken(this, secretKey);
        }
    }
}

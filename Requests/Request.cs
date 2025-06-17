using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

using NuciSecurity.HMAC;

namespace NuciAPI.Requests
{
    public abstract class Request()
    {
        [Required]
        [JsonPropertyName("hmac")]
        [HmacIgnore]
        public string HmacToken { get; set; }
    }
}

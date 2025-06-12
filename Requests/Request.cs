using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace NuciAPI.Requests
{
    public abstract class Request()
    {
        [Required]
        [JsonPropertyName("hmac")]
        public string HmacToken { get; set; }
    }
}

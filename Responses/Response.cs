using System.Text.Json.Serialization;
using NuciSecurity.HMAC;

namespace NuciAPI.Responses
{
    public abstract class Response(string message)
    {
        public abstract bool Success { get; }

        public string Message { get; set; } = message;

        [JsonPropertyName("hmac")]
        [HmacIgnore]
        public string HmacToken { get; set; }
    }
}

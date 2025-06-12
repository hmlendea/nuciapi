using System.Text.Json.Serialization;

namespace NuciAPI.Responses
{
    public abstract class Response(string message)
    {
        public abstract bool Success { get; }

        public string Message { get; set; } = message;

        [JsonPropertyName("hmac")]
        public string HmacToken { get; set; }
    }
}

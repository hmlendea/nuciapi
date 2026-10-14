using System.Text.Json;
using System.Text.Json.Serialization;

namespace NuciAPI.Responses
{
    /// <summary>
    /// Creates JSON serialisation options for NuciAPI contracts.
    /// </summary>
    public static class NuciApiJsonSerializerOptions
    {
        /// <summary>
        /// Creates options that omit null properties recursively during JSON serialisation.
        /// </summary>
        /// <returns>JSON serialisation options configured for NuciAPI contracts.</returns>
        public static JsonSerializerOptions Create()
            => new()
            {
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
            };
    }
}

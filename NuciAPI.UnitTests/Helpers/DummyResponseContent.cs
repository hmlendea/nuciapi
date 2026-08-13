using System.Text.Json.Serialization;

using NuciAPI.Responses;
using NuciSecurity.HMAC;

namespace NuciAPI.UnitTests.Helpers
{
    public sealed class DummyResponseContent : NuciApiResponseContent
    {
        [JsonIgnore]
        [HmacIgnore]
        public override bool IsEmpty => false;

        [HmacOrder(1)]
        public string DummyProperty { get; set; }
    }
}

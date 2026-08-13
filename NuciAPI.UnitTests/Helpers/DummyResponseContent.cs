using NuciAPI.Responses;
using NuciSecurity.HMAC;

namespace NuciAPI.UnitTests.Helpers
{
    public sealed class DummyResponseContent : NuciApiResponseContent
    {
        [HmacOrder(1)]
        public string DummyProperty { get; set; }
    }
}

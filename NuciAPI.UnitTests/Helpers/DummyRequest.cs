using NuciAPI.Requests;

namespace NuciAPI.UnitTests.Helpers
{
    public class DummyRequest : NuciApiRequest
    {
        public string DummyProperty { get; set; }
    }
}

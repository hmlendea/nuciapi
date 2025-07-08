using NuciAPI.Responses;

namespace NuciAPI.UnitTests.Helpers
{
    public class DummyResponse(string message) : NuciApiResponse(message)
    {
        public override bool IsSuccessful => true;

        public string DummyProperty { get; set; }
    }
}

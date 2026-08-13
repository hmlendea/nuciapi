using NuciAPI.Responses;

namespace NuciAPI.UnitTests.Helpers
{
    public class DummyResponse(string message) : NuciApiSuccessResponse(message, "DUMMY_CODE")
    {
        public override NuciApiResponseContent Content { get; set; } = new DummyResponseContent();
    }
}

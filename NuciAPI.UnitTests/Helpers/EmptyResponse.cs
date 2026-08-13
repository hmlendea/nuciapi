using NuciAPI.Responses;

namespace NuciAPI.UnitTests.Helpers
{
    public class EmptyResponse(string message) : NuciApiSuccessResponse(message, "TEST_CODE")
    {
        public override NuciApiResponseContent Content { get; set; }
    }
}

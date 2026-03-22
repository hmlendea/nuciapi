using NuciAPI.Responses;

namespace NuciAPI.UnitTests.Helpers
{
    public class EmptyResponse(string message) : NuciApiResponse(message, "TEST_CODE")
    {
        public override bool IsSuccessful => true;
    }
}

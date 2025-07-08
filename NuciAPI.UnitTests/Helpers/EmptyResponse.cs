using NuciAPI.Responses;

namespace NuciAPI.UnitTests.Helpers
{
    public class EmptyResponse(string message) : NuciApiResponse(message)
    {
        public override bool IsSuccessful => true;
    }
}

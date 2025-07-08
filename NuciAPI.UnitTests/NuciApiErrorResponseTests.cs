using NuciAPI.Responses;
using NUnit.Framework;

namespace NuciAPI.UnitTests
{
    public class NuciApiErrorResponseTests
    {
        [Test]
        public void GivenAnErrorResponse_WhenGettingTheIsSuccessfulProperty_ThenFalseIsReturned()
            => Assert.That(NuciApiErrorResponse.Default.IsSuccessful, Is.False);

        [Test]
        public void GivenAnErrorResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedMessageIsUsed()
            => Assert.That(NuciApiErrorResponse.Default.Message, Is.EqualTo(NuciApiResponseMessages.ErrorMessages.Default));

        [Test]
        public void GivenAnErrorResponse_WhenCreatingTheInvalidRequestResponse_ThenTheExpectedMessageIsUsed()
            => Assert.That(NuciApiErrorResponse.InvalidRequest.Message, Is.EqualTo(NuciApiResponseMessages.ErrorMessages.InvalidRequest));
    }
}
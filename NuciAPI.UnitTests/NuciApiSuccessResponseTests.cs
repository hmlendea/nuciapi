using NuciAPI.Responses;
using NUnit.Framework;

namespace NuciAPI.UnitTests
{
    public class NuciApiSuccessResponseTests
    {
        [Test]
        public void GivenASuccessResponse_WhenGettingTheIsSuccessfulProperty_ThenTrueIsReturned()
            => Assert.That(NuciApiSuccessResponse.Default.IsSuccessful, Is.True);

        [Test]
        public void GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedMessageIsUsed()
            => Assert.That(NuciApiSuccessResponse.Default.Message, Is.EqualTo(NuciApiResponseMessages.SuccessMessages.Default));
    }
}
using System.Text.Json;

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
        public void GivenAnErrorResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedCodeIsUsed()
            => Assert.That(NuciApiErrorResponse.Default.Code, Is.EqualTo(NuciApiResponseCodes.ErrorCodes.Default));

        [Test]
        public void GivenAnErrorResponse_WhenCreatingTheInvalidRequestResponse_ThenTheExpectedMessageIsUsed()
            => Assert.That(NuciApiErrorResponse.InvalidRequest.Message, Is.EqualTo(NuciApiResponseMessages.ErrorMessages.InvalidRequest));

        [Test]
        public void GivenAnErrorResponse_WhenSerialising_ThenTheContentPropertyIsAbsent()
        {
            NuciApiErrorResponse response = NuciApiErrorResponse.InvalidRequest;
            string serialisedResponse = JsonSerializer.Serialize(response);
            JsonDocument serialisedResponseDocument = JsonDocument.Parse(serialisedResponse);
            JsonElement rootElement = serialisedResponseDocument.RootElement;

            Assert.That(rootElement.TryGetProperty("content", out _), Is.False);
            Assert.That(rootElement.GetProperty("message").GetString(), Is.EqualTo(NuciApiResponseMessages.ErrorMessages.InvalidRequest));
            Assert.That(rootElement.GetProperty("code").GetString(), Is.EqualTo(NuciApiResponseCodes.ErrorCodes.InvalidRequest));
        }
    }
}
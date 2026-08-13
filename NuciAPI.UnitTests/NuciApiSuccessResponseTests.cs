using System.Text.Json;

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

        [Test]
        public void GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheExpectedCodeIsUsed()
            => Assert.That(NuciApiSuccessResponse.Default.Code, Is.EqualTo(NuciApiResponseCodes.SuccessCodes.Default));

        [Test]
        public void GivenASuccessResponse_WhenCreatingTheDefaultResponse_ThenTheContentIsNull()
            => Assert.That(NuciApiSuccessResponse.Default.Content, Is.Null);

        [Test]
        public void GivenASuccessResponse_WhenSerialising_ThenTheMessageAndCodePropertiesAreAtTheRootLevel()
        {
            NuciApiSuccessResponse response = NuciApiSuccessResponse.Default;
            string serialisedResponse = JsonSerializer.Serialize(response);
            JsonDocument serialisedResponseDocument = JsonDocument.Parse(serialisedResponse);
            JsonElement rootElement = serialisedResponseDocument.RootElement;

            Assert.That(rootElement.TryGetProperty("content", out JsonElement contentElement));
            Assert.That(rootElement.GetProperty("message").GetString(), Is.EqualTo(NuciApiResponseMessages.SuccessMessages.Default));
            Assert.That(rootElement.GetProperty("code").GetString(), Is.EqualTo(NuciApiResponseCodes.SuccessCodes.Default));
            Assert.That(contentElement.ValueKind, Is.EqualTo(JsonValueKind.Null));
        }
    }
}
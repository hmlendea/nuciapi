using System.Text.Json;

using NUnit.Framework;

using NuciAPI.Responses;
using NuciAPI.UnitTests.Helpers;

namespace NuciAPI.UnitTests
{
    [TestFixture]
    public sealed class NuciApiContentResponseTests
    {
        [Test]
        public void GivenContent_WhenCreatingAResponse_ThenTheContentIsRetained()
        {
            DummyResponseContent content = new()
            {
                DummyProperty = "Nucile rullz"
            };

            NuciApiContentResponse<DummyResponseContent> response = new(content);

            Assert.That(response.Content, Is.SameAs(content));
        }

        [Test]
        public void GivenContent_WhenCreatingAResponse_ThenTheDefaultMetadataIsUsed()
        {
            NuciApiContentResponse<DummyResponseContent> response = new(new());

            Assert.That(response.IsSuccessful);
            Assert.That(
                response.Message,
                Is.EqualTo(NuciApiResponseMessages.SuccessMessages.Default));
            Assert.That(
                response.Code,
                Is.EqualTo(NuciApiResponseCodes.SuccessCodes.Default));
        }

        [Test]
        public void GivenContent_WhenSerialisingAResponse_ThenTheContentPropertiesAreSerialised()
        {
            NuciApiContentResponse<DummyResponseContent> response = new(new()
            {
                DummyProperty = "Nucile rullz"
            });

            using JsonDocument responseDocument = JsonSerializer.SerializeToDocument(response);
            JsonElement contentElement = responseDocument.RootElement.GetProperty("content");

            Assert.That(
                contentElement.GetProperty("DummyProperty").GetString(),
                Is.EqualTo("Nucile rullz"));
        }

        [Test]
        public void GivenContent_WhenDeserialisingAResponse_ThenTheContentPropertiesAreDeserialised()
        {
            string serialisedResponse =
                "{\"success\":true,\"content\":{\"DummyProperty\":\"Nucile rullz\"}," +
                "\"message\":\"Operation completed successfully.\",\"code\":\"SUCCESS\"}";

            NuciApiContentResponse<DummyResponseContent> response =
                JsonSerializer.Deserialize<NuciApiContentResponse<DummyResponseContent>>(
                    serialisedResponse);

            Assert.That(response.Content.DummyProperty, Is.EqualTo("Nucile rullz"));
        }
    }
}

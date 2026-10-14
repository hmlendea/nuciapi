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
        public void GivenNullContent_WhenSerialisingAResponse_ThenTheContentPropertyIsOmitted()
        {
            NuciApiContentResponse<DummyResponseContent> response = new(null);

            using JsonDocument responseDocument = JsonSerializer.SerializeToDocument(response);

            Assert.That(responseDocument.RootElement.TryGetProperty("content", out _), Is.False);
        }

        [Test]
        public void GivenNestedNullProperties_WhenSerialisingWithNuciApiOptions_ThenNullPropertiesAreOmittedRecursively()
        {
            NuciApiContentResponse<NestedResponseContent> response = new(new()
            {
                Name = "Parent",
                Child = new()
                {
                    Name = "Child",
                    Grandchild = new()
                    {
                        Name = "Grandchild",
                        OptionalValue = null
                    },
                    OptionalValue = null
                },
                OptionalValue = null
            });

            using JsonDocument responseDocument = JsonSerializer.SerializeToDocument(
                response,
                NuciApiJsonSerializerOptions.Create());

            JsonElement contentElement = responseDocument.RootElement.GetProperty("content");
            JsonElement childElement = contentElement.GetProperty("Child");
            JsonElement grandchildElement = childElement.GetProperty("Grandchild");

            Assert.That(contentElement.GetProperty("Name").GetString(), Is.EqualTo("Parent"));
            Assert.That(childElement.GetProperty("Name").GetString(), Is.EqualTo("Child"));
            Assert.That(grandchildElement.GetProperty("Name").GetString(), Is.EqualTo("Grandchild"));
            Assert.That(contentElement.TryGetProperty("OptionalValue", out _), Is.False);
            Assert.That(childElement.TryGetProperty("OptionalValue", out _), Is.False);
            Assert.That(grandchildElement.TryGetProperty("OptionalValue", out _), Is.False);
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

        private sealed class NestedResponseContent : NuciApiResponseContent
        {
            public string Name { get; set; }

            public NestedResponseChild Child { get; set; }

            public string OptionalValue { get; set; }
        }

        private sealed class NestedResponseChild
        {
            public string Name { get; set; }

            public NestedResponseGrandchild Grandchild { get; set; }

            public string OptionalValue { get; set; }
        }

        private sealed class NestedResponseGrandchild
        {
            public string Name { get; set; }

            public string OptionalValue { get; set; }
        }
    }
}

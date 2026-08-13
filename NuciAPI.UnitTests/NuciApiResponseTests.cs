using NuciAPI.UnitTests.Helpers;
using NUnit.Framework;

namespace NuciAPI.UnitTests
{
    public class NuciApiResponseTests
    {
        private static string DummySecretKey => "DummySecretKey123!";

        [Test]
        public void GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated()
        {
            DummyResponse response = new("Test message")
            {
                Content = new DummyResponseContent
                {
                    DummyProperty = "Test value"
                }
            };

            Assert.That(response.HmacToken, Is.Null);

            response.SignHMAC(DummySecretKey);

            Assert.That(response.HmacToken, Is.Not.Null);
        }

        [Test]
        public void GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties()
        {
            string responseMessage = "Test message";

            EmptyResponse emptyResponse = new(responseMessage);
            DummyResponse response = new(responseMessage)
            {
                Content = new DummyResponseContent
                {
                    DummyProperty = "Test value"
                }
            };

            emptyResponse.SignHMAC(DummySecretKey);
            response.SignHMAC(DummySecretKey);

            Assert.That(response.HmacToken, Is.Not.EqualTo(emptyResponse.HmacToken));
        }
    }
}
using NuciAPI.UnitTests.Helpers;
using NUnit.Framework;

namespace NuciAPI.UnitTests
{
    public class NuciApiResponseTests
    {
        const string DummySecretKey = "DummySecretKey123!";

        [Test]
        public void GivenAResponse_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated()
        {
            DummyResponse response = new("Test message")
            {
                DummyProperty = "Test value"
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
                DummyProperty = "Test value"
            };

            emptyResponse.SignHMAC(DummySecretKey);
            response.SignHMAC(DummySecretKey);

            Assert.That(response.HmacToken, Is.Not.EqualTo(emptyResponse.HmacToken));
        }
    }
}
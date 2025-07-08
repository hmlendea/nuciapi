using NuciAPI.UnitTests.Helpers;
using NUnit.Framework;

namespace NuciAPI.UnitTests
{
    public class NuciApiRequestTests
    {
        const string DummySecretKey = "DummySecretKey123!";

        [Test]
        public void GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenIsPopulated()
        {
            DummyRequest request = new()
            {
                DummyProperty = "Test value"
            };

            Assert.That(request.HmacToken, Is.Null);

            request.SignHMAC(DummySecretKey);

            Assert.That(request.HmacToken, Is.Not.Null);
        }

        [Test]
        public void GivenARequest_WhenSigningTheHmac_ThenTheHmacTokenWasBuiltUsingAllProperties()
        {
            EmptyRequest emptyRequest = new();
            DummyRequest request = new()
            {
                DummyProperty = "Test value"
            };

            emptyRequest.SignHMAC(DummySecretKey);
            request.SignHMAC(DummySecretKey);

            Assert.That(request.HmacToken, Is.Not.EqualTo(emptyRequest.HmacToken));
        }
    }
}
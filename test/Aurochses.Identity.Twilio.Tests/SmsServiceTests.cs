using Microsoft.Extensions.Options;
using Moq;
using Xunit;

namespace Aurochses.Identity.Twilio.Tests
{
    public class SmsServiceTests
    {
        [Fact]
        public void Inherit_ISmsService()
        {
            // Arrange
            var mockTwilioOptions = new Mock<IOptions<TwilioOptions>>(MockBehavior.Strict);
            mockTwilioOptions
                .SetupGet(x => x.Value)
                .Returns(new TwilioOptions());

            // Act & Assert
            Assert.IsAssignableFrom<ISmsService>(new SmsService(mockTwilioOptions.Object));
        }
    }
}
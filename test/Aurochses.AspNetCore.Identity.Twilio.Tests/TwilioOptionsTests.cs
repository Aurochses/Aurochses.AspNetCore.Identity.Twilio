using Xunit;

namespace Aurochses.AspNetCore.Identity.Twilio.Tests
{
    public class TwilioOptionsTests
    {
        private readonly TwilioOptions _twilioOptions;

        public TwilioOptionsTests()
        {
            _twilioOptions = new TwilioOptions();
        }

        [Fact]
        public void FromPhoneNumber_Success()
        {
            // Arrange
            const string value = "fromPhoneNumber";

            // Act
            _twilioOptions.FromPhoneNumber = value;

            // Assert
            Assert.Equal(value, _twilioOptions.FromPhoneNumber);
        }

        [Fact]
        public void TwoFactorTokenBodyFormat_Success()
        {
            // Arrange
            const string value = "twoFactorTokenBodyFormat {0}";

            // Act
            _twilioOptions.TwoFactorTokenBodyFormat = value;

            // Assert
            Assert.Equal(value, _twilioOptions.TwoFactorTokenBodyFormat);
        }
    }
}
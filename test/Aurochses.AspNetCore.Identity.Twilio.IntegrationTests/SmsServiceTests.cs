using System.Threading.Tasks;
using Aurochses.AspNetCore.Identity.Twilio.IntegrationTests.Fakes;
using Microsoft.Extensions.Options;
using Twilio;
using Twilio.Exceptions;
using Xunit;

namespace Aurochses.AspNetCore.Identity.Twilio.IntegrationTests
{
    public class SmsServiceTests : IClassFixture<SmsServiceFixture>
    {
        private readonly SmsServiceFixture _fixture;

        public SmsServiceTests(SmsServiceFixture fixture)
        {
            _fixture = fixture;
        }

        private static IApplicationUser GetApplicationUser(string phoneNumber)
        {
            return new FakeApplicationUser
            {
                PhoneNumber = phoneNumber
            };
        }

        private SmsService GetSmsService(string fromPhoneNumber = null, string accountSid = null, string authToken = null)
        {
            TwilioClient.Init(accountSid ?? _fixture.Configuration["Twilio:AccountSid"], authToken ?? _fixture.Configuration["Twilio:AuthToken"]);

            var twilioOptions = new TwilioOptions
            {
                FromPhoneNumber = fromPhoneNumber ?? _fixture.Configuration["Identity:TwilioOptions:FromPhoneNumber"],
                TwoFactorTokenBodyFormat = _fixture.Configuration["Identity:TwilioOptions:TwoFactorTokenBodyFormat"]
            };

            return new SmsService(new OptionsWrapper<TwilioOptions>(twilioOptions));
        }

        [Theory]
        [InlineData("+15005550001", 21212)]
        [InlineData("+15005550007", 21606)]
        [InlineData("+15005550008", 21611)]
        public async Task SendTwoFactorTokenAsync_FromPhoneNumber_Failed(string fromPhoneNumber, int exceptionCode)
        {
            // Arrange
            var smsService = GetSmsService(fromPhoneNumber);

            var user = GetApplicationUser("+TestToPhoneNumber");

            // Act
            var sendResult = await smsService.SendTwoFactorTokenAsync(user, "TestToken");

            // Assert
            Assert.False(sendResult.Succeeded);
            var apiException = Assert.IsAssignableFrom<ApiException>(sendResult.Response);
            Assert.Equal(exceptionCode, apiException.Code);
        }

        [Theory]
        [InlineData("+15005550001", 21211)]
        [InlineData("+15005550002", 21612)]
        [InlineData("+15005550003", 21408)]
        [InlineData("+15005550004", 21610)]
        [InlineData("+15005550009", 21614)]
        public async Task SendTwoFactorTokenAsync_ToPhoneNumber_Failed(string toPhoneNumber, int exceptionCode)
        {
            // Arrange
            var smsService = GetSmsService();

            var user = GetApplicationUser(toPhoneNumber);

            // Act
            var sendResult = await smsService.SendTwoFactorTokenAsync(user, "TestToken");

            // Assert
            Assert.False(sendResult.Succeeded);
            var apiException = Assert.IsAssignableFrom<ApiException>(sendResult.Response);
            Assert.Equal(exceptionCode, apiException.Code);
        }

        [Fact]
        public async Task SendTwoFactorTokenAsync_IncorrectAccountSid()
        {
            // Arrange
            var smsService = GetSmsService(accountSid: "IncorrectAccountSid");

            var user = GetApplicationUser("+TestToPhoneNumber");

            // Act
            var sendResult = await smsService.SendTwoFactorTokenAsync(user, "TestToken");

            // Assert
            Assert.False(sendResult.Succeeded);
            var apiException = Assert.IsAssignableFrom<ApiException>(sendResult.Response);
            Assert.Equal(20404, apiException.Code);
        }

        [Fact]
        public async Task SendTwoFactorTokenAsync_IncorrectAuthToken()
        {
            // Arrange
            var smsService = GetSmsService(authToken: "IncorrectAuthToken");

            var user = GetApplicationUser("+TestToPhoneNumber");

            // Act
            var sendResult = await smsService.SendTwoFactorTokenAsync(user, "TestToken");

            // Assert
            Assert.False(sendResult.Succeeded);
            var apiException = Assert.IsAssignableFrom<ApiException>(sendResult.Response);
            Assert.Equal(20003, apiException.Code);
        }

        [Fact]
        public async Task SendTwoFactorTokenAsync_Success()
        {
            // Arrange
            var smsService = GetSmsService();

            var user = GetApplicationUser("+375297841506");

            // Act
            var sendResult = await smsService.SendTwoFactorTokenAsync(user, "TestToken");

            // Assert
            Assert.True(sendResult.Succeeded);
        }
    }
}
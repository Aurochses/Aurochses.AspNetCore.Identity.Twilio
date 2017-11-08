using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Twilio.Exceptions;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Aurochses.AspNetCore.Identity.Twilio
{
    /// <summary>
    /// Class SmsService.
    /// </summary>
    /// <seealso cref="Aurochses.AspNetCore.Identity.ISmsService" />
    public class SmsService : ISmsService
    {
        private readonly TwilioOptions _twilioOptions;

        /// <summary>
        /// Initializes a new instance of the <see cref="SmsService"/> class.
        /// </summary>
        /// <param name="twilioOptions">The twilio options.</param>
        public SmsService(IOptions<TwilioOptions> twilioOptions)
        {
            _twilioOptions = twilioOptions.Value;
        }

        /// <summary>
        /// Send Two Factor Token
        /// </summary>
        /// <param name="user">The User.</param>
        /// <param name="token">The token.</param>
        /// <returns>SendResult</returns>
        public async Task<SendResult> SendTwoFactorTokenAsync(IApplicationUser user, string token)
        {
            try
            {
                var messageResource = await MessageResource.CreateAsync(
                    new PhoneNumber(user.PhoneNumber),
                    @from: new PhoneNumber(_twilioOptions.FromPhoneNumber),
                    body: string.Format(_twilioOptions.TwoFactorTokenBodyFormat, token)
                );

                return messageResource.ErrorCode.HasValue
                    ? SendResult.Failed(messageResource)
                    : SendResult.Success;
            }
            catch (ApiException exception)
            {
                return SendResult.Failed(exception);
            }
        }
    }
}
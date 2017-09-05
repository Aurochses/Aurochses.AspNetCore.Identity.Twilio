namespace Aurochses.Identity.Twilio
{
    /// <summary>
    /// Class TwilioOptions.
    /// </summary>
    public class TwilioOptions
    {
        /// <summary>
        /// Gets or sets from phone number.
        /// </summary>
        /// <value>
        /// From phone number.
        /// </value>
        public string FromPhoneNumber { get; set; }

        /// <summary>
        /// Gets or sets the two factor token body format.
        /// </summary>
        /// <value>
        /// The two factor token body format.
        /// </value>
        public string TwoFactorTokenBodyFormat { get; set; }
    }
}
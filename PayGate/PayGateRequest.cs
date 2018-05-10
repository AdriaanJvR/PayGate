using System;
using System.Collections.Specialized;
using System.Text;
using System.Web;

namespace PayGate
{
    public class PayGateRequest : PayGateBase
    {
        #region Constructor
        public PayGateRequest(string passPhrase) : base(passPhrase)
        {
        }
        #endregion

        // TODO: Implement enums (structs?) for country, locale, pay method, etc.
        // TODO: Indicate which properties are required.

        #region Properties
        /// <summary>
        /// Your PayGateID – assigned by PayGate.
        /// Number(11)
        /// </summary>
        public string PAYGATE_ID { get; set; }

        /// <summary>
        /// This is your reference number for use by your internal systems
        /// </summary>
        public string REFERENCE { get; set; }

        /// <summary>
        /// Transaction amount in cents.
        /// </summary>
        public int AMOUNT { get; set; }

        /// <summary>
        /// Currency code of the currency the customer is paying in.
        /// </summary>
        public string CURRENCY { get; set; }

        /// <summary>
        /// Once the transaction is completed, PayWeb will return the customer to a page on your web site. 
        /// The page the customer must see is specified in this field.
        /// </summary>
        public string RETURN_URL { get; set; }

        /// <summary>
        /// This is the date that the transaction was initiated on your website or system. 
        /// The transaction date must be specified in 'Coordinated Universal Time' (UTC) and formatted as ‘YYYY-MM-DD HH:MM:SS’
        /// </summary>
        public DateTime TRANSACTION_DATE { get; set; }

        /// <summary>
        /// The locale code identifies to PayGate the customer’s language, country and any special variant preferences (such as Date/Time format) which may be applied to the user interface.
        /// </summary>
        public string LOCALE { get; set; }

        /// <summary>
        /// Country code of the country the customer is paying from.
        /// </summary>
        public string COUNTRY { get; set; }

        /// <summary>
        /// If the transaction is approved, PayWeb will email a payment confirmation to this email address – unless this is overridden at a gateway level by using the Payment Confirmation setting.
        /// </summary>
        public string EMAIL { get; set; }

        /// <summary>
        /// The payment method(s) to show to the client.
        /// </summary>
        public string PAY_METHOD { get; set; }

        /// <summary>
        /// The PAY_METHOD_DETAIL field should be left blank unless the merchant has more than one active payment method 
        /// and wants to make sure that the client is presented with a specific payment method.
        /// </summary>
        public string PAY_METHOD_DETAIL { get; set; }

        /// <summary>
        /// If the notify URL field is populated, then PayWeb will post the fields to the notify URL immediately when the transaction is completed.
        /// </summary>
        public string NOTIFY_URL { get; set; }

        /// <summary>
        /// This field is optional and has been included as a placeholder for merchant specific requirements.
        /// </summary>
        public string USER1 { get; set; }

        /// <summary>
        /// This field is optional and has been included as a placeholder for merchant specific requirements.
        /// </summary>
        public string USER2 { get; set; }

        /// <summary>
        /// This field is optional and has been included as a placeholder for merchant specific requirements.
        /// </summary>
        public string USER3 { get; set; }

        /// <summary>
        /// This field is optional but should only be included if PayVault credit card tokenisation is enabled on the merchant profile.
        /// </summary>
        public bool? VAULT { get; set; }

        /// <summary>
        /// This field is optional and should only be included if PayVault credit card tokenisation is enabled.
        /// </summary>
        public string VAULT_ID { get; set; }


        public string CHECKSUM => CreateHash();
        #endregion

        #region Overrides
        public override string ToString()
        {
            var stringBuilder = this.GetPropertyValueString();

            var securityHash = CreateHash();

            if (string.IsNullOrWhiteSpace(this.passPhrase) && !stringBuilder.ToString().EndsWith("&"))
            {
                stringBuilder.Append($"&CHECKSUM={HttpUtility.UrlEncode(securityHash)}");
            }
            else
            {
                stringBuilder.Append($"CHECKSUM={HttpUtility.UrlEncode(securityHash)}");
            }

            return stringBuilder.ToString();
        }
        #endregion

        #region Private Methods


        private StringBuilder GetPropertyValueString()
        {
            var stringBuilder = new StringBuilder();
            var nameValueCollection = this.GetNameValueCollection();

            var lastEntryKey = this.DetermineLast(nameValueCollection);

            foreach (string key in nameValueCollection)
            {
                var value = nameValueCollection[key];

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }

                if (string.IsNullOrWhiteSpace(this.passPhrase) && key == lastEntryKey)
                {
                    stringBuilder.Append($"{key}={HttpUtility.UrlEncode(value)}");
                }
                else
                {
                    stringBuilder.Append($"{key}={HttpUtility.UrlEncode(value)}&");
                }
            }

            return stringBuilder;
        }

        private string DetermineLast(NameValueCollection nameValueCollection)
        {
            string lastKey = nameValueCollection.GetKey(nameValueCollection.Count - 1);

            foreach (string key in nameValueCollection)
            {
                var value = nameValueCollection[key];

                if (string.IsNullOrWhiteSpace(value))
                {
                    continue;
                }
                else
                {
                    lastKey = key;
                }
            }

            return lastKey;
        }
        #endregion
    }
}

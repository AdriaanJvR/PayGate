using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace PayGate
{
    public class PayGateResponse : PayGateBase
    {
        #region Constructor
        public PayGateResponse(string passPhrase) : base(passPhrase)
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// This should be the same PayGate ID that was passed in the request; if it is not, then the data has been altered.
        /// </summary>
        public string PAYGATE_ID { get; set; }

        /// <summary>
        /// The PAY_REQUEST_ID is a GUID allocated by PayWeb to the transaction request received from the merchant.
        /// </summary>
        public string PAY_REQUEST_ID { get; set; }

        /// <summary>
        /// The reference that was passed in the original request is returned unaltered.
        /// </summary>
        public string REFERENCE { get; set; }

        public string CHECKSUM => CreateHash();
        #endregion

        #region Overrides
        public override string ToString()
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append($"PAY_REQUEST_ID={HttpUtility.UrlEncode(PAY_REQUEST_ID)}");
            stringBuilder.Append($"&CHECKSUM={HttpUtility.UrlEncode(CHECKSUM)}");

            return stringBuilder.ToString();
        }
        #endregion

        #region Methods
        public void MapResponse(Dictionary<string, string> response)
        {
            // TODO: Make this generic.
            PAYGATE_ID = response["PAYGATE_ID"];
            PAY_REQUEST_ID = response["PAY_REQUEST_ID"];
            REFERENCE = response["REFERENCE"];
        }
        #endregion
    }
}

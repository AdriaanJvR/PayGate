using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace PayGate
{
    public class PayGateReturn : PayGateBase
    {
        #region Fields
        private readonly Dictionary<string, string> properties;
        #endregion

        #region Constructor
        /// <summary>
        /// This constructor does not allow for a argument
        /// because it is intended to be called by the mvc model binder.
        /// If a passphrase is being used, make a call to SetPassPhrase(string passPhrase)
        /// on this class after it has been passed in by the model binder.
        /// </summary>
        public PayGateReturn() : base(string.Empty)
        {
            properties = new Dictionary<string, string>();
        }
        #endregion

        #region Properties
        /// <summary>
        /// The PAY_REQUEST_ID returned by PayWeb after the first request.
        /// </summary>
        public string PAY_REQUEST_ID
        {
            get
            {
                return properties.ValueAs(nameof(PAY_REQUEST_ID));
            }
            set
            {
                properties.AddOrUpdate(nameof(PAY_REQUEST_ID), value);
            }
        }
        
        /// <summary>
        /// The final status of the transaction.
        /// </summary>
        public TransactionStatus TRANSACTION_STATUS
        {
            get
            {
                string transactionStatus = properties.ValueAs(nameof(TRANSACTION_STATUS));
                return (TransactionStatus)Enum.Parse(typeof(TransactionStatus), transactionStatus);
            }
            set
            {
                properties.AddOrUpdate(nameof(TRANSACTION_STATUS), ((int)value).ToString());
            }
        }

        /// <summary>
        /// This field contains a calculated MD5 hash based on the values of the PAYGATE_ID, PAY_REQUEST_ID, TRANSACTION_STATUS, REFERENCE fields and a key.
        /// </summary>
        public string CHECKSUM
        {
            get
            {
                return properties.ValueAs(nameof(CHECKSUM));
            }
            set
            {
                properties.AddOrUpdate(nameof(CHECKSUM), value);
            }
        }
        #endregion

        #region Methods
        public void FromFormCollection(IEnumerable<KeyValuePair<string, string>> nameValueCollection)
        {
            if (nameValueCollection == null || nameValueCollection.Count() < 1)
            {
                return;
            }

            foreach (KeyValuePair<string, string> keyValuePair in nameValueCollection)
            {
                this.properties.AddOrUpdate(key: keyValuePair.Key, value: keyValuePair.Value);
            }
        }

        public string GetCalculatedChecksum(string payGateID, string payRequestID, string reference)
        {
            var stringBuilder = new StringBuilder();

            stringBuilder.Append(payGateID);
            stringBuilder.Append(payRequestID);
            stringBuilder.Append(((int)TRANSACTION_STATUS).ToString());
            stringBuilder.Append(reference);
            stringBuilder.Append(passPhrase);

            return CreateHash(stringBuilder);
        }
        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;

namespace PayGate
{
    public class PayGateBase
    {
        #region Fields
        protected string passPhrase;
        #endregion

        #region Constructor
        public PayGateBase(string passPhrase)
        {
            this.passPhrase = passPhrase;
        }
        #endregion

        #region Methods
        public void SetPassPhrase(string passPhrase)
        {
            this.passPhrase = passPhrase;
        }

        public NameValueCollection GetNameValueCollection()
        {
            var collection = new NameValueCollection();

            this.GetType().GetProperties()
                .Where(x => x.Name != "CHECKSUM")
                .ToList()
                .ForEach(x => collection.Add(x.Name, this.GetPropertyValue(x)));

            return collection;
        }

        private string GetPropertyValue(PropertyInfo propertyInfo)
        {
            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(bool)))
            {
                if (propertyInfo.PropertyType == typeof(bool?))
                {
                    var booleanValue = (bool?)propertyInfo.GetValue(this, null);

                    return booleanValue.HasValue ? booleanValue.Value ? "1" : "0" : string.Empty;
                }
                else
                {
                    var booleanValue = (bool)propertyInfo.GetValue(this, null);

                    return booleanValue ? "1" : "0";
                }
            }

            if (propertyInfo.PropertyType.IsAssignableFrom(typeof(DateTime)))
            {
                if (propertyInfo.PropertyType == typeof(DateTime?))
                {
                    var dateTimeValue = (DateTime?)propertyInfo.GetValue(this, null);

                    return dateTimeValue.ToPayGateDate();
                }
                else
                {
                    var dateTimeValue = (DateTime)propertyInfo.GetValue(this, null);

                    return dateTimeValue.ToPayGateDate();
                }
            }

            return propertyInfo.GetValue(this, null) == null ? string.Empty : propertyInfo.GetValue(this, null).ToString();
        }

        public string CreateHash()
        {
            var input = new StringBuilder();
            var nameValueCollection = GetNameValueCollection();

            foreach (string key in nameValueCollection)
            {
                var value = nameValueCollection[key];

                if (!string.IsNullOrWhiteSpace(value))
                {
                    input.Append(value);
                }
            }

            input.Append(passPhrase);

            return CreateHash(input);
        }

        public string CreateHash(StringBuilder input)
        {
            StringBuilder stringBuilder = new StringBuilder();

            using (MD5 md5Hash = MD5.Create())
            {
                byte[] hash = md5Hash.ComputeHash(Encoding.UTF8.GetBytes(input.ToString()));

                for (int i = 0; i < hash.Length; i++)
                {
                    stringBuilder.Append(hash[i].ToString("x2"));
                }
            }
            return stringBuilder.ToString();
        }
        #endregion
    }
}

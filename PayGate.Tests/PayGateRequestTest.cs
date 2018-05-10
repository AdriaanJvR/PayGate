using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayGate.Tests
{
    [TestClass]
    public class PayGateRequestTest
    {
        [TestMethod]
        public void ChecksumWithAllOptionalFieldsEmpty()
        {
            // Arrange
            var secret = "secret";
            var payGateRequest = new PayGateRequest(secret);

            payGateRequest.PAYGATE_ID = "10011072130";
            payGateRequest.REFERENCE = "PayGate Test";
            payGateRequest.AMOUNT = 3299;
            payGateRequest.CURRENCY = "ZAR";
            payGateRequest.RETURN_URL = "https://www.paygate.co.za/thankyou";
            payGateRequest.TRANSACTION_DATE = DateTime.Parse("2016-03-10 10:49:16");
            payGateRequest.LOCALE = "en";
            payGateRequest.COUNTRY = "ZAF";
            payGateRequest.EMAIL = "customer@paygate.co.za";

            // Act
            var checksum = payGateRequest.CHECKSUM;

            // Assert
            Assert.AreEqual("0bcaea6fa6bc0337e066db9826088557", checksum);
        }
    }
}

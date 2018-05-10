using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayGate.Tests
{
    [TestClass]
    public class PayGateReturnTest
    {
        [TestMethod]
        public void VerifyGeneratedChecksum()
        {
            // Arrange
            var payGateReturn = new PayGateReturn();
            payGateReturn.SetPassPhrase("secret");

            Dictionary<string, string> formCollection = new Dictionary<string, string>();
            formCollection.Add("PAYGATE_ID", "10011072130");
            formCollection.Add("PAY_REQUEST_ID", "26F1EE9D-FB68-D6C2-5D36-ADA8C5F88BC9");
            formCollection.Add("TRANSACTION_STATUS", "1");
            formCollection.Add("REFERENCE", "PayGate Test");

            payGateReturn.FromFormCollection(formCollection);

            // Act
            var checksum = payGateReturn.GetCalculatedChecksum(formCollection["PAYGATE_ID"], formCollection["PAY_REQUEST_ID"], formCollection["REFERENCE"]);

            // Assert
            Assert.AreEqual("2fae4c5cde9ac8ed70f769c3ff843d72", checksum);
        }
    }
}

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PayGate.Tests
{
    [TestClass]
    public class PayGateResponseTest
    {
        [TestMethod]
        public void CreateResponseChecksum()
        {
            // Arrange
            var secret = "secret";
            var payGateResponse = new PayGateResponse(secret);

            payGateResponse.PAYGATE_ID = "10011072130";
            payGateResponse.PAY_REQUEST_ID = "26F1EE9D-FB68-D6C2-5D36-ADA8C5F88BC9";
            payGateResponse.REFERENCE = "PayGate Test";

            // Act
            var checksum = payGateResponse.CHECKSUM;

            // Assert
            Assert.AreEqual("f5563213b72cb405167ba53e8c3ee466", checksum);
        }
    }
}

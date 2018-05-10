namespace PayGate
{
    public class PayGateSettings
    {
        public string PayGateID { get; set; }
        public string PayGateKey { get; set; }
        public string InitiateURL { get; set; }
        public string ProcessURL { get; set; }
        public string Locale { get; set; }
        public string Country { get; set; }
        public string Currency { get; set; }
        public string ReturnURL { get; set; }
        public string NotifyURL { get; set; }
    }
}

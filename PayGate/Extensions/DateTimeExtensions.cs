namespace System
{
    public static class DateTimeExtensions
    {
        public static string ToPayGateDate(this DateTime dateTime)
        {
            return dateTime.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static string ToPayGateDate(this DateTime? dateTime)
        {
            if (dateTime.HasValue)
            {
                return dateTime.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }

            return string.Empty;
        }
    }
}

using System;

namespace Domain
{
    public static class DateTimeExtensions
    {
        public static bool IsFutureDate(this DateTime datum)
        {
            return datum > DateTime.UtcNow;
        }

        public static bool IsBeforeOrEqual(this DateTime datum, DateTime compare)
        {
            return datum <= compare;
        }
    }
}
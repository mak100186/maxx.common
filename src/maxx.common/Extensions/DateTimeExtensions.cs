namespace maxx.common.Extensions
{
    using System;

    public static class DateTimeExtensions
    {
        public static bool IsInFuture(this DateTime source)
        {
            return source > DateTime.UtcNow;
        }
    }
}

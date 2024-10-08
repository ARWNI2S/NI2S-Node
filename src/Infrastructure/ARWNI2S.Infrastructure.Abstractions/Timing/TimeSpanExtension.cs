﻿namespace ARWNI2S.Infrastructure.Timing
{
    /// <summary>
    /// The Utils class contains a variety of utility methods for use in application and entity code.
    /// </summary>
    internal static class TimeSpanExtension
    {
        public static TimeSpan Multiply(this TimeSpan timeSpan, double value)
        {
            double ticksD = checked(timeSpan.Ticks * value);
            long ticks = checked((long)ticksD);
            return TimeSpan.FromTicks(ticks);
        }

        public static TimeSpan Divide(this TimeSpan timeSpan, double value)
        {
            double ticksD = checked(timeSpan.Ticks / value);
            long ticks = checked((long)ticksD);
            return TimeSpan.FromTicks(ticks);
        }

        public static double Divide(this TimeSpan first, TimeSpan second)
        {
            double ticks1 = first.Ticks;
            double ticks2 = second.Ticks;
            return ticks1 / ticks2;
        }

        public static TimeSpan Max(TimeSpan first, TimeSpan second)
        {
            return first >= second ? first : second;
        }

        public static TimeSpan Min(TimeSpan first, TimeSpan second)
        {
            return first < second ? first : second;
        }
    }
}

using System;

namespace NI2S.Node.Infrastructure
{
    internal static class StringExtensions
    {
        public static bool AsBool(this string value)
        {
            return string.Equals("true", value, StringComparison.OrdinalIgnoreCase)
                || string.Equals("1", value, StringComparison.OrdinalIgnoreCase);
        }
    }
}

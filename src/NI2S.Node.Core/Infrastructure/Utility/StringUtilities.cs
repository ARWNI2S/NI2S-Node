using System;

namespace NI2S.Node.Infrastructure
{
    internal sealed class StringUtilities
    {
        public static bool ParseBool(string value)
        {
            return string.Equals("true", value, StringComparison.OrdinalIgnoreCase)
                || string.Equals("1", value, StringComparison.OrdinalIgnoreCase);
        }
    }
}

using System;

namespace NI2S.Node.Hosting
{
    internal sealed class WebHostUtilities
    {
        public static bool ParseBool(string? value)
        {
            return string.Equals("true", value, StringComparison.OrdinalIgnoreCase)
                || string.Equals("1", value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
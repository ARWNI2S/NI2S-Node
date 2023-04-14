// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;

namespace NI2S.Node
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
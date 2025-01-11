// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Configuration
{
    public static class ConfigurationStringExtensions
    {
        public static bool ParseBool(this string value)
        {
            return string.Equals("true", value, StringComparison.OrdinalIgnoreCase)
                || string.Equals("1", value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
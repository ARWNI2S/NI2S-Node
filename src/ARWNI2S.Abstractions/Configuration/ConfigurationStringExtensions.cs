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
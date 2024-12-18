namespace ARWNI2S.Node.Hosting
{
    public static class HostingConfigurationExtensions
    {
        public static bool ParseBool(this string value)
        {
            return string.Equals("true", value, StringComparison.OrdinalIgnoreCase)
                || string.Equals("1", value, StringComparison.OrdinalIgnoreCase);
        }
    }
}
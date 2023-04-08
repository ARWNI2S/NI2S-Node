namespace NI2S.Engine
{
    public class CacheKey
    {
        private string v;
        private string adminNavigationPluginsPrefix;

        public CacheKey(string v, string adminNavigationPluginsPrefix)
        {
            this.v = v;
            this.adminNavigationPluginsPrefix = adminNavigationPluginsPrefix;
        }
    }
}

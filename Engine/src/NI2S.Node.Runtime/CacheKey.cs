using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node
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

using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Node.Runtime.Configuration
{
    public class RuntimeConfig : IConfig
    {
        /// <inheritdoc/>
        public int GetOrder() => 4;
    }
}

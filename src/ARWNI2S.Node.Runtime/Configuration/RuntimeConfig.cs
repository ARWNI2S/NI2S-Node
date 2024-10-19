using ARWNI2S.Infrastructure.Configuration;

namespace ARWNI2S.Runtime.Configuration
{
    public class RuntimeConfig : IConfig
    {
        /// <inheritdoc/>
        public int GetOrder() => 4;
    }
}

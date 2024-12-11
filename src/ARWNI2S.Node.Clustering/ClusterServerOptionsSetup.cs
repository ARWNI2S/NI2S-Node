using ARWNI2S.Clustering.Configuration;
using ARWNI2S.Configuration;
using Microsoft.Extensions.Options;

namespace ARWNI2S.Clustering
{
    internal class ClusterServerOptionsSetup : IConfigureOptions<ClusterServerOptions>
    {
        private readonly ClusterConfig _clusterConfig;

        public ClusterServerOptionsSetup(NI2SSettings settings)
        {
            _clusterConfig = settings.Get<ClusterConfig>();
        }

        public void Configure(ClusterServerOptions options)
        {
            options.ClusterId = _clusterConfig.ClusterId;
            options.ConnectionString = _clusterConfig.ConnectionString;
            options.IsDevelopment = _clusterConfig.IsDevelopment;
            options.KnownNodes = _clusterConfig.KnownNodes;
            options.ServiceId = _clusterConfig.ServiceId;
            options.SiloStorageClustering = _clusterConfig.SiloStorageClustering;

        }
    }
}
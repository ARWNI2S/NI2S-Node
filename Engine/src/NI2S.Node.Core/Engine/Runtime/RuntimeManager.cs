using NI2S.Node.Data;
using NI2S.Node.Engine.Runtime;
using System.Collections.Generic;

namespace NI2S.Node.Engine
{
    internal class RuntimeManager : IClusterManager
    {
        List<IDataSource> dataSources;

        public List<IDataSource> GetDataSources()
        {
            if (dataSources == null || EngineRuntime.RefreshData)
                dataSources = EngineRuntime.LoadClusterDataSources();

            return dataSources;
        }
    }
}

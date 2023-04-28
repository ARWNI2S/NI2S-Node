using NI2S.Node.Data;
using System.Collections.Generic;

namespace NI2S.Node
{
    public interface IClusterManager
    {
        List<IDataSource> GetDataSources();
    }
}

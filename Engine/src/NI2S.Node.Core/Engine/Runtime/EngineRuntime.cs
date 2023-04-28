using NI2S.Node.Data;
using Orleans.Metadata;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Engine.Runtime
{
    internal sealed class EngineRuntime
    {
        #region Constants

        private const int MAX_POOL_LIMIT = 1024 * 1024;

        #endregion

        #region Static Class

        public static bool RefreshData => !instance.ClusterReady;

        internal static List<IDataSource> LoadClusterDataSources()
        {
            throw new NotImplementedException();
        }


        #endregion

        #region Always Singleton
        private static EngineRuntime instance;

        private EngineRuntime() { }

        #endregion

        #region Fields

        private ClusterState _clusterState;

        #endregion

        public bool ClusterReady
        {
            get
            {
                _clusterState=CheckClusterState();
                return instance._clusterState == ClusterState.Ready;
            }
        }

        private ClusterState CheckClusterState()
        {
            ClusterState result = ClusterState.Unknown;

            if (_clusterState == ClusterState.Ready)
            {
                // TODO: check all cluster is up to date...
                result = _clusterState;
            }
            else
            {

            }

            return result;
        }


    }
}

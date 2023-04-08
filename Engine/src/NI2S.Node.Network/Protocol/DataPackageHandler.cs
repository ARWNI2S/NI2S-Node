// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network;
using System;
using System.Threading.Tasks;

namespace NI2S.Node.Network.Protocol
{
    internal class DataPackageHandler : IPackageHandler<DataPackage>
    {
        public ValueTask Handle(IAppSession session, DataPackage package)
        {
            throw new NotImplementedException();
        }
    }
}

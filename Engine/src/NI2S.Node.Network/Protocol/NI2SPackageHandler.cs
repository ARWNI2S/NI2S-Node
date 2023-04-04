// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network;
using System;
using System.Threading.Tasks;

namespace NI2S.Node.Network.Protocol
{
    internal class NI2SPackageHandler : IPackageHandler<NI2SPackage>
    {
        public ValueTask Handle(IAppSession session, NI2SPackage package)
        {
            throw new NotImplementedException();
        }
    }
}

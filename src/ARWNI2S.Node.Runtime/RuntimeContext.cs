using ARWNI2S.Node.Core.Network;
using ARWNI2S.Node.Core.Runtime;

namespace ARWNI2S.Node.Runtime
{
    internal class RuntimeContext : IExecutionContext
    {
        public IRuntimeRequest Request => throw new NotImplementedException();

        public INodeConnection Connection => throw new NotImplementedException();
    }
}

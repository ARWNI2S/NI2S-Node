using ARWNI2S.Engine.Network.Protocol;
using ARWNI2S.Engine.Network.Session;
using System.Text;

namespace ARWNI2S.Runtime.Console
{
    public abstract class NodeCommandBase : INodeCommand
    {
        public async ValueTask ExecuteAsync(INodeSession session, StringPackageInfo package, CancellationToken cancellationToken)
        {
            var result = package.Parameters
                .Select(p => int.Parse(p))
                .Sum();

            await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"), cancellationToken);
        }
    }
}

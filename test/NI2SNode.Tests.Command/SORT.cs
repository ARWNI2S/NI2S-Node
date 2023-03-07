using NI2S.Node.Command;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;
using System.Text;

namespace NI2S.Node.Tests.Command
{
    public class SORT : IAsyncCommand<ISession, StringPackageInfo>
    {
        public async ValueTask ExecuteAsync(ISession session, StringPackageInfo package)
        {
            var result = string.Join(' ', package.Parameters!
                .Select(p => int.Parse(p)).OrderBy(x => x).Select(x => x.ToString()));

            await session.SendAsync(Encoding.UTF8.GetBytes($"{nameof(SORT)} {result}\r\n"));
        }
    }
}

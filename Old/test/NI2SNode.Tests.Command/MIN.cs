using NI2S.Node.Command;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;
using System.Text;

namespace NI2S.Node.Tests.Command
{
    public class MIN : IAsyncCommand<ISession, StringPackageInfo>
    {
        public async ValueTask ExecuteAsync(ISession session, StringPackageInfo package)
        {
            var result = package.Parameters!
                .Select(p => int.Parse(p)).OrderBy(x => x).FirstOrDefault();

            await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
        }
    }
}

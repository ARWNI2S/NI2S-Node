using NI2S.Node.Command;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Session;
using System.Text;

namespace NI2S.Node.Tests
{
    public class MySession : Session
    {

    }

    public class ADD : IAsyncCommand<StringPackageInfo>
    {
        public async ValueTask ExecuteAsync(ISession session, StringPackageInfo package)
        {
            var result = package.Parameters
                .Select(p => int.Parse(p))
                .Sum();

            await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
        }
    }

    public class MULT : IAsyncCommand<StringPackageInfo>
    {
        public async ValueTask ExecuteAsync(ISession session, StringPackageInfo package)
        {
            var result = package.Parameters
                .Select(p => int.Parse(p))
                .Aggregate((x, y) => x * y);

            await session.SendAsync(Encoding.UTF8.GetBytes(result.ToString() + "\r\n"));
        }
    }

    public class SUB : IAsyncCommand<StringPackageInfo>
    {
        private readonly IPackageEncoder<string> _encoder;

        public SUB(IPackageEncoder<string> encoder)
        {
            _encoder = encoder;
        }

        public async ValueTask ExecuteAsync(ISession session, StringPackageInfo package)
        {
            var result = package.Parameters
                .Select(p => int.Parse(p))
                .Aggregate((x, y) => x - y);

            // encode the text message by encoder
            await session.SendAsync(_encoder, result.ToString() + "\r\n");
        }
    }

    public class DIV : IAsyncCommand<MySession, StringPackageInfo>
    {
        private readonly IPackageEncoder<string> _encoder;

        public DIV(IPackageEncoder<string> encoder)
        {
            _encoder = encoder;
        }

        public async ValueTask ExecuteAsync(MySession session, StringPackageInfo package)
        {
            var values = package
                .Parameters
                .Select(p => int.Parse(p))
                .ToArray();

            var result = values[0] / values[1];

            var socketSession = session as ISession;
            // encode the text message by encoder
            await socketSession.SendAsync(_encoder, result.ToString() + "\r\n");
        }
    }

    public class PowData
    {
        public int X { get; set; }
        public int Y { get; set; }
    }

    public class POW : JsonAsyncCommand<ISession, PowData>
    {
        protected override async ValueTask ExecuteJsonAsync(ISession session, PowData data)
        {
            await session.SendAsync(Encoding.UTF8.GetBytes($"{Math.Pow(data.X, data.Y)}\r\n"));
        }
    }

    public class MaxData
    {
        public int[] Numbers { get; set; }
    }

    public class MAX : JsonAsyncCommand<ISession, MaxData>
    {
        protected override async ValueTask ExecuteJsonAsync(ISession session, MaxData data)
        {
            var maxValue = data.Numbers.OrderByDescending(i => i).FirstOrDefault();
            await session.SendAsync(Encoding.UTF8.GetBytes($"{maxValue}\r\n"));
        }
    }
}
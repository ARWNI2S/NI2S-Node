using NI2S.Node.Protocol;
using System.Text;

namespace NI2S.Node.Configuration.Options
{
    public class ServerOptions : ChannelOptions
    {
        public string? Name { get; set; }

        public List<ListenOptions>? Listeners { get; set; }

        public Encoding? DefaultTextEncoding { get; set; }

        public int ClearIdleSessionInterval { get; set; } = 120;

        public int IdleSessionTimeOut { get; set; } = 300;
    }
}
using ARWNI2S.Infrastructure.Network.Connection;

namespace ARWNI2S.Infrastructure
{
    public interface IContext
    {
        IServiceProvider ServiceProvider { get; }

        IMessage Message { get; }

        IMessage Response { get; }

        IConnection Connection { get; }

        Dictionary<string, object> Items { get; }
    }
}

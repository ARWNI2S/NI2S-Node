using NI2S.Node.Protocol.Session;

namespace NI2S.Node.Command
{
    public interface ICommand
    {
        // empty interface
    }

    public interface ICommand<TPackageInfo> : ICommand<ISession, TPackageInfo>
    {

    }

    public interface ICommand<TAppSession, TPackageInfo> : ICommand
        where TAppSession : ISession
    {
        void Execute(TAppSession session, TPackageInfo package);
    }

    public interface IAsyncCommand<TPackageInfo> : IAsyncCommand<ISession, TPackageInfo>
    {

    }

    public interface IAsyncCommand<TAppSession, TPackageInfo> : ICommand
        where TAppSession : ISession
    {
        ValueTask ExecuteAsync(TAppSession session, TPackageInfo package);
    }
}

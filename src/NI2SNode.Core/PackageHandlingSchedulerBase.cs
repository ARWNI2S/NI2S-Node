using NI2S.Node;
using NI2S.Node.Protocol.Channel;

namespace SuperSocket.Server
{
    public abstract class PackageHandlingSchedulerBase<TPackageInfo> : IPackageHandlingScheduler<TPackageInfo>
    {
        public IPackageHandler<TPackageInfo>? PackageHandler { get; private set; }

        public Func<IAppSession, PackageHandlingException<TPackageInfo>, ValueTask<bool>>? ErrorHandler { get; private set; }

        public abstract ValueTask HandlePackage(IAppSession session, TPackageInfo package);

        public virtual void Initialize(IPackageHandler<TPackageInfo> packageHandler, Func<IAppSession, PackageHandlingException<TPackageInfo>, ValueTask<bool>> errorHandler)
        {
            PackageHandler = packageHandler;
            ErrorHandler = errorHandler;
        }

        protected async ValueTask HandlePackageInternal(IAppSession session, TPackageInfo package)
        {
            var packageHandler = PackageHandler;
            var errorHandler = ErrorHandler;
            if (errorHandler == null) throw new InvalidOperationException(nameof(errorHandler)); //TODO: Message

            try
            {
                if (packageHandler != null)
                    await packageHandler.Handle(session, package);
            }
            catch (Exception e)
            {
                var toClose = await errorHandler(session, new PackageHandlingException<TPackageInfo>($"Session {session.SessionID} got an error when handle a package.", package, e));

                if (toClose)
                {
                    session.CloseAsync(CloseReason.ApplicationError).DoNotAwait();
                }
            }
        }
    }
}
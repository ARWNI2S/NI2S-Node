using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public abstract class PackageHandlingSchedulerBase<TPackageInfo> : IPackageHandlingScheduler<TPackageInfo>
    {
        public IPackageHandler<TPackageInfo>? PackageHandler { get; private set; }

        public Func<ISession, PackageHandlingException<TPackageInfo>, ValueTask<bool>>? ErrorHandler { get; private set; }

        public abstract ValueTask HandlePackage(ISession session, TPackageInfo package);

        public virtual void Initialize(IPackageHandler<TPackageInfo> packageHandler, Func<ISession, PackageHandlingException<TPackageInfo>, ValueTask<bool>> errorHandler)
        {
            PackageHandler = packageHandler;
            ErrorHandler = errorHandler;
        }

        protected async ValueTask HandlePackageInternal(ISession session, TPackageInfo package)
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
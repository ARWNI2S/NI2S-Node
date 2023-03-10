using NI2S.Node.Protocol.Session;

namespace NI2S.Node
{
    public class DelegatePackageHandler<TReceivePackageInfo> : IPackageHandler<TReceivePackageInfo>
    {
        readonly Func<ISession, TReceivePackageInfo, ValueTask> _func;

        public DelegatePackageHandler(Func<ISession, TReceivePackageInfo, ValueTask> func)
        {
            _func = func;
        }

        public async ValueTask Handle(ISession session, TReceivePackageInfo package)
        {
            await _func(session, package);
        }
    }
}
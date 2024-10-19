using ARWNI2S.Node.Core.Network;
using ARWNI2S.Node.Core.Runtime;
using ARWNI2S.Node.Core.Runtime.Features;
using System.Security.Claims;

namespace ARWNI2S.Runtime
{
    internal class RuntimeContext : IExecutionContext
    {
        public IFeatureCollection Features => throw new NotImplementedException();

        public HttpRequest Request => throw new NotImplementedException();

        public HttpResponse Response => throw new NotImplementedException();

        public ConnectionInfo Connection => throw new NotImplementedException();

        public NodeSocketManager WebSockets => throw new NotImplementedException();

        public ClaimsPrincipal User { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IDictionary<object, object> Items { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public IServiceProvider ContextServices { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public CancellationToken RequestAborted { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string TraceIdentifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ISession Session { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    }
}

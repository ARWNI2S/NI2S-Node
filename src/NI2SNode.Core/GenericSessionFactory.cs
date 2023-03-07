using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Protocol.Session;
using System;

namespace NI2S.Node.Server
{
    public class GenericSessionFactory<TSession> : ISessionFactory
        where TSession : ISession
    {
        public Type SessionType => typeof(TSession);

        public IServiceProvider ServiceProvider { get; private set; }

        public GenericSessionFactory(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }

        public ISession Create()
        {
            return ActivatorUtilities.CreateInstance<TSession>(ServiceProvider);
        }
    }
}
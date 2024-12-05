using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace ARWNI2S.Node.Hosting.Startup
{
    internal sealed class ConventionBasedStartup : INodeStartup
    {
        private readonly StartupMethods _methods;

        public ConventionBasedStartup(StartupMethods methods)
        {
            _methods = methods;
        }

        public void Configure(IEngineBuilder engine)
        {
            try
            {
                _methods.ConfigureDelegate(engine);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
                throw;
            }
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                return _methods.ConfigureServicesDelegate(services);
            }
            catch (TargetInvocationException ex)
            {
                ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
                throw;
            }
        }
    }
}
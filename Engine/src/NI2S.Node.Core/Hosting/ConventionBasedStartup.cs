using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Builder;
using System;
using System.Reflection;
using System.Runtime.ExceptionServices;

namespace NI2S.Node.Hosting
{
    internal sealed class ConventionBasedStartup : IStartup
    {
        private readonly StartupMethods _methods;

        public ConventionBasedStartup(StartupMethods methods)
        {
            _methods = methods;
        }

        public void Configure(IEngineBuilder app)
        {
            try
            {
                _methods.ConfigureDelegate(app);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
                }

                throw;
            }
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                return _methods.ConfigureServicesDelegate(services);
            }
            catch (Exception ex)
            {
                if (ex is TargetInvocationException)
                {
                    ExceptionDispatchInfo.Capture(ex.InnerException!).Throw();
                }

                throw;
            }
        }
    }
}
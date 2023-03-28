using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;
using NI2S.Node.Hosting.Infrastructure;
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

        public void Configure(INodeBuilder node)
        {
            try
            {
                _methods.ConfigureDelegate(node);
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

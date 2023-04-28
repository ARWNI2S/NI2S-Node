// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NI2S.Node.Core.Infrastructure
{
    /// <summary>
    /// Represents NI2S engine
    /// </summary>
    public partial class NI2SEngine : IEngine
    {
        #region Utilities

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        protected virtual IServiceProvider GetServiceProvider(IServiceScope scope = null)
        {
            if (scope == null)
            {
                var accessor = ServiceProvider.GetService<IEngineContextAccessor>();
                var context = accessor?.EngineContext;
                return context?.NodeServices ?? ServiceProvider;
            }
            return scope.ServiceProvider;
        }

        /// <summary>
        /// Run startup tasks
        /// </summary>
        /* 002.3.5.2 - ConfigureNodeEngineBuilder(...) -> builder.Services.ConfigureEngineServices(...) -> engine.ConfigureServices(...) -> AddAutoMapper() */
        protected virtual void RunStartupTasks()
        {
            //find startup tasks provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

            //create and sort instances of startup tasks
            //we startup this interface even for not installed plugins. 
            //otherwise, DbContext initializers won't run and a plugin installation won't work
            var instances = startupTasks
                .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
                .OrderBy(startupTask => startupTask.Order);

            //execute tasks
            foreach (var task in instances)
                task.Execute();
        }

        /// <summary>
        /// Register and configure AutoMapper
        /// </summary>
        /* 002.3.5.1 - ConfigureNodeEngineBuilder(...) -> builder.Services.ConfigureEngineServices(...) -> engine.ConfigureServices(...) -> AddAutoMapper() */
        protected virtual void AddAutoMapper()
        {
            ////find mapper configurations provided by other assemblies
            //var typeFinder = Singleton<ITypeFinder>.Instance;
            //var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

            ////create and sort instances of mapper configurations
            //var instances = mapperConfigurations
            //    .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
            //    .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            ////create AutoMapper configuration
            //var config = new MapperConfiguration(cfg =>
            //{
            //    foreach (var instance in instances)
            //    {
            //        cfg.AddProfile(instance.GetType());
            //    }
            //});

            ////register
            //AutoMapperConfiguration.Init(config);
        }

        protected virtual Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        {
            //check for assembly already loaded
            var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            if (assembly != null)
                return assembly;

            //get assembly from TypeFinder
            var typeFinder = Singleton<ITypeFinder>.Instance;
            assembly = typeFinder?.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
            return assembly;
        }

        #endregion

        #region Methods

        /// <inheritdoc/>
        /* 002.3.5 - ConfigureNodeEngineBuilder(...) -> builder.Services.ConfigureEngineServices(...) -> engine.ConfigureServices(...) */
        public virtual void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //register engine
            services.AddSingleton<IEngine>(this);

            //find startup configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var startupConfigurations = typeFinder.FindClassesOfType<INodeStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (INodeStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);

            services.AddSingleton(services);

            //register mapper configurations
            AddAutoMapper();

            //run startup tasks
            RunStartupTasks();

            //resolve assemblies here. otherwise, plugins can throw an exception when rendering views
            AppDomain.CurrentDomain.AssemblyResolve += CurrentDomain_AssemblyResolve;
        }

        /// <inheritdoc/>
        public virtual void ConfigureEventHandler(IEngineBuilder engine)
        {
            ServiceProvider = engine.EngineServices;

            //find startup configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var startupConfigurations = typeFinder.FindClassesOfType<INodeStartup>();

            //create and sort instances of startup configurations
            var instances = startupConfigurations
                .Select(startup => (INodeStartup)Activator.CreateInstance(startup))
                .OrderBy(startup => startup.Order);

            //configure request pipeline
            foreach (var instance in instances)
                instance.Configure(engine);
        }

        /// <inheritdoc/>
        public virtual T Resolve<T>(IServiceScope scope = null) where T : class
        {
            return (T)Resolve(typeof(T), scope);
        }

        /// <inheritdoc/>
        public virtual object Resolve(Type type, IServiceScope scope = null)
        {
            return GetServiceProvider(scope)?.GetService(type);
        }

        /// <inheritdoc/>
        public virtual IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

        /// <inheritdoc/>
        public virtual object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType);
                        return service ?? throw new NI2SException("Unknown dependency");
                    });

                    //all is ok, so create instance
                    return Activator.CreateInstance(type, parameters.ToArray());
                }
                catch (Exception ex)
                {
                    innerException = ex;
                }
            }

            throw new NI2SException("No constructor was found that had all the dependencies satisfied.", innerException);
        }

        #endregion

        #region Properties

        /// <inheritdoc/>
        public virtual IServiceProvider ServiceProvider { get; protected set; }

        /// <inheritdoc/>
        public virtual IModuleCollection Modules { get; protected set; }

        #endregion
    }
}
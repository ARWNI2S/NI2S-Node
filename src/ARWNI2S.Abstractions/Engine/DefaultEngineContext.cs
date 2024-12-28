using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine
{
    /// <summary>
    /// Provides access to the singleton instance of the node engine context.
    /// </summary>
    internal sealed class DefaultEngineContext : IEngineContext
    {
        #region Utilities

        /// <summary>
        /// Get IServiceProvider
        /// </summary>
        /// <returns>IServiceProvider</returns>
        private IServiceProvider GetServiceProvider(IServiceScope scope = null)
        {
            if (scope == null)
            {
                var accessor = ServiceProvider?.GetService<INiisContextAccessor>();
                var context = accessor?.NI2SContext;
                return context?.ServiceProvider ?? ServiceProvider;
            }
            return scope.ServiceProvider;
        }

        ///// <summary>
        ///// Run startup tasks
        ///// </summary>
        //private void RunStartupTasks()
        //{
        //    //HACK
        //    ////find startup tasks provided by other assemblies
        //    //var typeFinder = Singleton<ITypeFinder>.Instance;
        //    //var startupTasks = typeFinder.FindClassesOfType<IPreInitTask>();

        //    ////create and sort instances of startup tasks
        //    ////we startup this interface even for not installed modules. 
        //    ////otherwise, DbContext initializers won't run and a module installation won't work
        //    //var instances = startupTasks
        //    //    .Select(startupTask => (IPreInitTask)Activator.CreateInstance(startupTask))
        //    //    .OrderBy(startupTask => startupTask.Order);

        //    ////execute tasks
        //    //foreach (var task in instances)
        //    //    task.Execute();
        //}

        ///// <summary>
        ///// Register and configure AutoMapper
        ///// </summary>
        //private void AddAutoMapper()
        //{
        //    //HACK
        //    ////find mapper configurations provided by other assemblies
        //    //var typeFinder = Singleton<ITypeFinder>.Instance;
        //    //var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

        //    ////create and sort instances of mapper configurations
        //    //var instances = mapperConfigurations
        //    //    .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
        //    //    .OrderBy(mapperConfiguration => mapperConfiguration.Order);

        //    ////create AutoMapper configuration
        //    //var config = new MapperConfiguration(cfg =>
        //    //{
        //    //    foreach (var instance in instances)
        //    //    {
        //    //        cfg.AddProfile(instance.GetType());
        //    //    }
        //    //});

        //    ////register
        //    //AutoMapperConfiguration.Init(config);
        //}

        //private Assembly CurrentDomain_AssemblyResolve(object sender, ResolveEventArgs args)
        //{
        //    //check for assembly already loaded
        //    var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
        //    if (assembly != null)
        //        return assembly;

        //    //get assembly from TypeFinder
        //    var typeFinder = Singleton<ITypeFinder>.Instance;
        //    assembly = typeFinder?.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
        //    return assembly;
        //}

        #endregion

        #region Methods

        public void InitializeContext(IEngineBuilder engine)
        {
            ServiceProvider = engine.EngineServices;
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="scope">Scope</param>
        /// <typeparam name="T">Type of resolved service</typeparam>
        /// <returns>Resolved service</returns>
        public T Resolve<T>(IServiceScope scope = null) where T : class
        {
            return (T)Resolve(typeof(T), scope);
        }

        /// <summary>
        /// Resolve dependency
        /// </summary>
        /// <param name="type">Type of resolved service</param>
        /// <param name="scope">Scope</param>
        /// <returns>Resolved service</returns>
        public object Resolve(Type type, IServiceScope scope = null)
        {
            return GetServiceProvider(scope)?.GetService(type);
        }

        /// <summary>
        /// Resolve dependencies
        /// </summary>
        /// <typeparam name="T">Type of resolved services</typeparam>
        /// <returns>Collection of resolved services</returns>
        public IEnumerable<T> ResolveAll<T>()
        {
            return (IEnumerable<T>)GetServiceProvider().GetServices(typeof(T));
        }

        /// <summary>
        /// Resolve unregistered service
        /// </summary>
        /// <param name="type">Type of service</param>
        /// <returns>Resolved service</returns>
        public object ResolveUnregistered(Type type)
        {
            Exception innerException = null;
            foreach (var constructor in type.GetConstructors())
            {
                try
                {
                    //try to resolve constructor parameters
                    var parameters = constructor.GetParameters().Select(parameter =>
                    {
                        var service = Resolve(parameter.ParameterType) ?? throw new NI2SException("Unknown dependency");
                        return service;
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

        /// <summary>
        /// Service provider
        /// </summary>
        public IServiceProvider ServiceProvider { get; protected set; }

        #endregion
    }
}

using ARWNI2S.Engine.Environment.Mapper;
using ARWNI2S.Environment;
using ARWNI2S.Extensibility;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine
{
    public static class EngineContextExtensions
    {
        /// <summary>
        /// Add and configure services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public static void ConfigureServices(this IEngineContext context, IServiceCollection services, IConfiguration configuration)
        {
            //register engine
            services.AddSingleton(context);

            //find startup configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var modules = typeFinder.FindClassesOfType<IEngineModule>();

            //create and sort instances of startup configurations
            var instances = modules
                .Select(startup => (IEngineModule)Activator.CreateInstance(startup))
                .Where(startup => startup != null)
                .OrderBy(startup => startup.Order);

            //configure services
            foreach (var instance in instances)
                instance.ConfigureServices(services, configuration);

            services.AddSingleton(services);

            //register mapper configurations
            AddAutoMapper();

            ////run startup tasks
            //RunStartupTasks();

            //resolve assemblies here. otherwise, plugins can throw an exception when rendering views
            AppDomain.CurrentDomain.AssemblyResolve += (object sender, ResolveEventArgs args) =>
            {
                //check for assembly already loaded
                var assembly = AppDomain.CurrentDomain.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                if (assembly != null)
                    return assembly;

                //get assembly from TypeFinder
                var typeFinder = Singleton<ITypeFinder>.Instance;
                assembly = typeFinder?.GetAssemblies().FirstOrDefault(a => a.FullName == args.Name);
                return assembly;
            };
        }

        /// <summary>
        /// Register and configure AutoMapper
        /// </summary>
        private static void AddAutoMapper()
        {
            //find mapper configurations provided by other assemblies
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var mapperConfigurations = typeFinder.FindClassesOfType<IOrderedMapperProfile>();

            //create and sort instances of mapper configurations
            var instances = mapperConfigurations
                .Select(mapperConfiguration => (IOrderedMapperProfile)Activator.CreateInstance(mapperConfiguration))
                .OrderBy(mapperConfiguration => mapperConfiguration.Order);

            //create AutoMapper configuration
            var config = new MapperConfiguration(cfg =>
            {
                foreach (var instance in instances)
                {
                    cfg.AddProfile(instance.GetType());
                }
            });

            //register
            AutoMapperConfiguration.Init(config);
        }

        ///// <summary>
        ///// Run startup tasks
        ///// </summary>
        //private void RunStartupTasks()
        //{
        //    //find startup tasks provided by other assemblies
        //    var typeFinder = Singleton<ITypeFinder>.Instance;
        //    var startupTasks = typeFinder.FindClassesOfType<IStartupTask>();

        //    //create and sort instances of startup tasks
        //    //we startup this interface even for not installed modules. 
        //    //otherwise, DbContext initializers won't run and a module installation won't work
        //    var instances = startupTasks
        //        .Select(startupTask => (IStartupTask)Activator.CreateInstance(startupTask))
        //        .OrderBy(startupTask => startupTask.Order);

        //    //execute tasks
        //    foreach (var task in instances)
        //        task.Execute();
        //}
    }
}

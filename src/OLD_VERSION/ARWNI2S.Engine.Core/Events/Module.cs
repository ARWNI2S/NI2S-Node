using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Engine.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Events
{
    public class Module : IEngineModule
    {
        /// <summary>
        /// Add and configure any of the engine services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<IEventPublisher, EventPublisher>();

            //event consumers
            var consumers = Singleton<ITypeFinder>.Instance.FindClassesOfType(typeof(IEventConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IEventConsumer<>)))
                    services.AddScoped(findInterface, consumer);
        }

        /// <summary>
        /// Configure the using of added components
        /// </summary>
        /// <param name="engine">Builder for configuring a node's NI2S engine</param>
        public void ConfigureEngine(IEngineBuilder engine)
        {

        }

        ///// <summary>
        ///// Gets order of this startup configuration implementation
        ///// </summary>
        //public int Order => InitStage.DbInit;
    }
}

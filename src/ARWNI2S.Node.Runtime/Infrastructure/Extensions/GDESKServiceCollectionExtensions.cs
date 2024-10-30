using ARWNI2S.Engine.Simulation;
using ARWNI2S.Engine.Simulation.Time;
using ARWNI2S.Infrastructure.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ARWNI2S.Runtime.Infrastructure.Extensions
{
    public static class GDESKServiceCollectionExtensions
    {
        public static void UseSimulationClock<TImplementation>(this IServiceCollection services) where TImplementation : class, ISimulationClock
        {
            if (services.Any(x => x.ServiceType == typeof(ISimulationClock)))
            {
                services.RemoveAll<ISimulationClock>();
            }

            services.TryAddSingleton<TImplementation>();
            services.AddFromExisting(typeof(ISimulationClock), typeof(TImplementation));
        }

        public static void UseSimulation<TImplementation>(this IServiceCollection services) where TImplementation : class, ISimulation
        {
            if (services.Any(x => x.ServiceType == typeof(ISimulation)))
            {
                services.RemoveAll<ISimulation>();
            }

            services.TryAddSingleton<TImplementation>();
            services.AddFromExisting(typeof(ISimulation), typeof(TImplementation));
        }
    }
}

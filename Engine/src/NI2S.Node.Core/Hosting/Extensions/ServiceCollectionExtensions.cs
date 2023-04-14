// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.DependencyInjection;

namespace NI2S.Node.Hosting
{
    internal static class ServiceCollectionExtensions
    {
        public static IServiceCollection Clone(this IServiceCollection serviceCollection)
        {
            IServiceCollection clone = new ServiceCollection();
            foreach (var service in serviceCollection)
            {
                clone.Add(service);
            }
            return clone;
        }
    }
}

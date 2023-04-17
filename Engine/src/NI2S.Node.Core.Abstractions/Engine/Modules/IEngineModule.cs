// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Hosting.Builder;

namespace NI2S.Node.Engine
{
    public interface IEngineModule
    {
        ModuleDescriptorBase ModuleDescriptor { get; set; }

        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the node engine</param>
        void ConfigureServices(IServiceCollection services, IConfiguration configuration);

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="engine">Builder for configuring a NI2S node engine.</param>
        void Configure(INodeEngineBuilder engine);

    }
}

using DragonCorp.Metalink.Core.Infrastructure;
using DragonCorp.Metalink.Server.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonCorp.Metalink.Server.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring endpoints on application startup
    /// </summary>
    public partial class MetalinkEndpoints : IMetalinkStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //Endpoints routing
            application.UseDraCoEndpoints();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 900; //authentication should be loaded before MVC
    }
}

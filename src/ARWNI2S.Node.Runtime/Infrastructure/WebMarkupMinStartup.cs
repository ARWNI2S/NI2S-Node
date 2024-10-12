﻿using DragonCorp.Metalink.Core.Infrastructure;
using DragonCorp.Metalink.Server.Framework.Infrastructure.Extensions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DragonCorp.Metalink.Server.Framework.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring WebMarkupMin services on application startup
    /// </summary>
    public partial class WebMarkupMinStartup : IMetalinkStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add WebMarkupMin services to the services container
            services.AddDraCoWebMarkupMin();
        }

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IApplicationBuilder application)
        {
            //use WebMarkupMin
            application.UseDraCoWebMarkupMin();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 300; //Ensure that "UseDraCoWebMarkupMin" method is invoked before "UseRouting". Otherwise, HTML minification won't work
    }
}

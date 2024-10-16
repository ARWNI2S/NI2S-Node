using ARWNI2S.Infrastructure;
using ARWNI2S.Node.Runtime.Infrastructure.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Runtime.Infrastructure
{
    /// <summary>
    /// Represents object for the configuring common features and middleware on application startup
    /// </summary>
    public partial class CommonStartup : INI2SStartup
    {
        /// <summary>
        /// Add and configure any of the middleware
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //add options feature
            services.AddOptions();

            //add distributed cache
            services.AddDistributedCache();

            //add HTTP sesion state feature
            //services.AddHttpSession();

            //add default HTTP clients
            //services.AddDraCoHttpClients();

            //add anti-forgery
            //services.AddAntiForgery();

            //add routing
            //services.AddRouting(options =>
            //{
            //    //add constraint key for language
            //    options.ConstraintMap[PortalRoutingDefaults.LanguageParameterTransformer] = typeof(LanguageParameterTransformer);
            //});
        }

        ///// <summary>
        ///// Configure the using of added middleware
        ///// </summary>
        ///// <param name="application">Builder for configuring an application's request pipeline</param>
        //public void Configure(IApplicationBuilder application)
        //{
        //    //check whether requested page is keep alive page
        //    application.UseKeepAlive();

        //    //check whether database is installed
        //    application.UseInstallUrl();

        //    //use HTTP session
        //    application.UseSession();

        //    //use request localization
        //    application.UseDraCoRequestLocalization();
        //}

        /// <summary>
        /// Configure the using of added middleware
        /// </summary>
        /// <param name="application">Builder for configuring an application's request pipeline</param>
        public void Configure(IHost application)
        {
            //check whether requested page is keep alive page
            //application.UseKeepAlive();

            //check whether database is installed
            //application.UseInstallUrl();

            //use HTTP session
            //application.UseSession();

            //use request localization
            //application.UseDraCoRequestLocalization();
        }

        /// <summary>
        /// Gets order of this startup configuration implementation
        /// </summary>
        public int Order => 100; //common services should be loaded after error handlers
    }
}
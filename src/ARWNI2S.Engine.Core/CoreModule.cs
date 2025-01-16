using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Configuration;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Environment;
using ARWNI2S.Lifecycle;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine
{
    internal class CoreModule : FrameworkModule
    {
        public override int Order => NI2SLifecycleStage.RuntimeInitialize - 1;

        public override string SystemName => "NI2S.Core";

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //////add options feature
            ////services.AddOptions();

            //////add distributed cache
            ////services.AddDistributedCache();

            ////add HTTP session state feature
            //services.AddHttpSession();

            ////add default HTTP clients
            //services.AddNopHttpClients();

            ////add anti-forgery
            //services.AddAntiForgery();

            ////add theme support
            //services.AddThemes();

            ////add routing
            //services.AddRouting(options =>
            //{
            //    //add constraint key for language
            //    options.ConstraintMap[NopRoutingDefaults.LanguageParameterTransformer] = typeof(LanguageParameterTransformer);
            //});

            //register the Lazy resolver for .Net IoC
            var useAutofac = Settings.Get<CommonConfig>().UseAutofac;
            if (!useAutofac)
                services.AddScoped(typeof(Lazy<>), typeof(LazyInstance<>));

        }

        public override void ConfigureEngine(IEngineBuilder engineBuilder)
        {
            ////check whether requested page is keep alive page
            //application.UseKeepAlive();

            ////check whether database is installed
            //application.UseInstallUrl();

            ////use HTTP session
            //application.UseSession();

            ////use request localization
            //application.UseNopRequestLocalization();

            ////configure PDF
            //application.UseNopPdf();

            //engineBuilder.EngineServices.GetRequiredService<IModuleManager>().FrameworkModules[typeof(CoreModule)] = this;
            base.ConfigureEngine(engineBuilder);
        }
    }
}

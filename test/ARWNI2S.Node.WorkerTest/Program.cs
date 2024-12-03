namespace ARWNI2S.Node.WorkerTest
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //var niisHost = NI2SNodeHost.Create(args);

            //niisHost.ConfigureLocalNodeRuntime();
            //await niisHost.StartEngineAsync();

            //await niisHost.RunAsync();
            await Task.CompletedTask;
        }

        //public static void Main(string[] args)
        //{
        //    var builder = Host.CreateApplicationBuilder(args);
        //    //builder.Services.AddHostedService<Worker>();

        //    var host = builder.Build();
        //    host.Run();
        //}

        //public static async Task Main(string[] args)
        //{
        //var builder = NodeEngine.CreateBuilder(args);
        //var builder = Host.CreateDefaultBuilder(args);



        //builder.Configuration.AddJsonFile(NiisConfigurationDefaults.SettingsFilePath, true, true);
        //if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
        //{
        //    var path = string.Format(NiisConfigurationDefaults.SettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
        //    builder.Configuration.AddJsonFile(path, true, true);
        //}
        //builder.Configuration.AddEnvironmentVariables();

        //load engine settings
        //builder.Services.ConfigureEngineSettings(builder);

        //var niisSettings = Singleton<NiisSettings>.Instance;
        //var useAutofac = niisSettings.Get<CommonConfig>().UseAutofac;

        //if (useAutofac)
        //    builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
        //else
        //    builder.Host.UseDefaultServiceProvider(options =>
        //    {
        //        //we don't validate the scopes, since at the app start and the initial configuration we need 
        //        //to resolve some services (registered as "scoped") through the root container
        //        options.ValidateScopes = false;
        //        options.ValidateOnBuild = true;
        //    });

        ////add services to the engine and configure service provider
        //builder.Services.ConfigureEngineServices(builder);

        //var host = builder.Build();

        ////configure the engine HTTP request pipeline
        //host.ConfigureRequestPipeline();
        //await host.StartEngineAsync();

        //await host.RunAsync();
        //}
    }
}
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Runtime.Infrastructure.Hosting;
using Autofac.Extensions.DependencyInjection;
//using ARWNI2S.Node.Hosting.Extensions;

namespace ARWNI2S.Node
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            //    await NI2SNode
            //        .MinimalNode(args)
            //        .Build()
            //        .RunAsync();
            //}




            //var builder = NodeHostBuilder.Create(args);
            var builder = Host.CreateDefaultBuilder(args);

            // Configurar los archivos de configuración
            builder.ConfigureAppConfiguration((hostingContext, config) =>
            {
                var environment = hostingContext.HostingEnvironment;
                config.AddJsonFile(ConfigurationDefaults.NodeSettingsFilePath, optional: true, reloadOnChange: true);

                if (!string.IsNullOrEmpty(environment.EnvironmentName))
                {
                    var path = string.Format(ConfigurationDefaults.NodeSettingsEnvironmentFilePath, environment.EnvironmentName);
                    config.AddJsonFile(path, optional: true, reloadOnChange: true);
                }

                config.AddEnvironmentVariables();
            });

            // Configurar servicios de la aplicación y ajustes
            builder.ConfigureServices((context, services) =>
            {
                // Load application settings
                services.ConfigureApplicationSettings(context);

                var nodeSettings = Singleton<NodeSettings>.Instance;
                var useAutofac = nodeSettings.Get<CommonConfig>().UseAutofac;

                if (useAutofac)
                    builder.UseServiceProviderFactory(new AutofacServiceProviderFactory());
                else
                    builder.UseDefaultServiceProvider(options =>
                    {
                        //we don't validate the scopes, since at the app start and the initial configuration we need 
                        //to resolve some services (registered as "scoped") through the root container
                        options.ValidateScopes = false;
                        options.ValidateOnBuild = true;
                    });

                services.ConfigureApplicationServices(context);
            });

            // Construir y ejecutar el host
            var host = builder.Build();

            //await host.StartEngineAsync();

            await host.RunAsync();
        }
            //static void Main(string[] args)
            //{
            //    // Variables to store the parsed option values
            //    bool showHelp = false;
            //    string name = "DefaultName";
            //    int repeat = 1;
            //    List<string> files = new List<string>();
            //    string logLevel = "info";
            //    bool verbose = false;

            //    // Define the options
            //    var options = new OptionSet()
            //{
            //    { "n|name=", "The {NAME} of the user (default is 'DefaultName').", v => name = v },
            //    { "r|repeat=", "How many times to repeat the message (default is 1).", (int v) => repeat = v },
            //    { "f|file=", "A {FILE} to process (can be specified multiple times).", v => files.Add(v) },
            //    { "l|loglevel=", "Set the {LOGLEVEL} (default is 'info').", v => logLevel = v },
            //    { "v|verbose", "Enable verbose output.", v => verbose = v != null },
            //    { "h|help", "Show this message and exit.", v => showHelp = v != null },
            //};

            //    // Parse the command-line arguments
            //    List<string> extra;
            //    try
            //    {
            //        extra = options.Parse(args);
            //    }
            //    catch (OptionException e)
            //    {
            //        Console.WriteLine($"Error: {e.Message}");
            //        Console.WriteLine("Try '--help' for more information.");
            //        return;
            //    }

            //    // Show help message if requested
            //    if (showHelp)
            //    {
            //        ShowHelp(options);
            //        return;
            //    }

            //    // Output the parsed values
            //    Console.WriteLine($"Name: {name}");
            //    Console.WriteLine($"Repeat: {repeat}");
            //    Console.WriteLine($"Log Level: {logLevel}");
            //    Console.WriteLine($"Verbose: {verbose}");
            //    Console.WriteLine("Files to process:");
            //    foreach (var file in files)
            //    {
            //        Console.WriteLine($"  {file}");
            //    }

            //    // Handle extra arguments
            //    if (extra.Count > 0)
            //    {
            //        Console.WriteLine("Extra arguments:");
            //        foreach (string e in extra)
            //        {
            //            Console.WriteLine(e);
            //        }
            //    }

            //    // Sample logic: Repeat the name message
            //    for (int i = 0; i < repeat; i++)
            //    {
            //        Console.WriteLine($"Hello, {name}!");
            //    }
            //}

            // Method to show the help message
            //static void ShowHelp(OptionSet options)
            //{
            //    Console.WriteLine("Usage: app [OPTIONS]+");
            //    Console.WriteLine("Options:");
            //    options.WriteOptionDescriptions(Console.Out);
            //}

        }
    }
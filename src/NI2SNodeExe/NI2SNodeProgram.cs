using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using NI2S.Node.Hosting;

namespace NI2S.Node
{
    internal class NI2SNodeProgram
    {
        public static readonly string ExeName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        public static readonly string ExePath = Path.GetFullPath(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Node launcher program entry point.
        /// </summary>
        static void Main(string[] args)
        {
            var builder = NI2SNodeHostBuilder.CreateDefaultBuilder(args);

            //builder.conf


            if (args.Length == 0)
            {
                //Console run run
            }

            var host = NI2SNodeHostBuilder.CreateDefaultBuilder(args)
                .UseSomeService(async (s) =>
                {
                    //DO SOMETHING
                    await ValueTask.CompletedTask;
                })
                //.ConfigureSuperSocket(options =>
                //{
                //    options.Name = "Echo Server";
                //    options.AddListener(new ListenOptions
                //    {
                //        Ip = "Any",
                //        Port = 4040
                //    }
                //    );
                //})
                .ConfigureLogging((hostCtx, loggingBuilder) =>
                {
                    loggingBuilder.AddConsole();
                })
                .Build();







            //var builder = WebApplication.CreateBuilder(args);

            //builder.Configuration.AddJsonFile(NopConfigurationDefaults.AppSettingsFilePath, true, true);
            //if (!string.IsNullOrEmpty(builder.Environment?.EnvironmentName))
            //{
            //    var path = string.Format(NopConfigurationDefaults.AppSettingsEnvironmentFilePath, builder.Environment.EnvironmentName);
            //    builder.Configuration.AddJsonFile(path, true, true);
            //}
            //builder.Configuration.AddEnvironmentVariables();
        }



























        //public int Run(string[] args, TextWriter? logger = null)
        //{
        //    if (logger == null)
        //    {
        //        logger = Console.Out;
        //    }

        //    bool showHelp;
        //    string outputFilePath;

        //    OptionSet p;
        //    List<string> inputFiles;
        //    var app = NI2SNodeApp.CreateNI2SNodeApp(args, logger, out p, out showHelp/*, out outputFilePath*/, out inputFiles);
        //    if (showHelp)
        //    {
        //        p.WriteOptionDescriptions(logger);
        //        return 1;
        //    }

        //    if (inputFiles.Count != 1)
        //    {
        //        p.WriteOptionDescriptions(logger);
        //        return ExitWithError("This tool requires one input file.", logger);
        //    }

        //    var inputFile = inputFiles[0];

        //    // Add search path from input file
        //    //app.SearchDirectories.Add(Path.GetDirectoryName(inputFile));

        //    // Load symbol file if it exists
        //    var symbolFile = Path.ChangeExtension(inputFile, "pdb");
        //    if (File.Exists(symbolFile))
        //    {
        //        app.UseSymbols = true;
        //    }

        //    // Setup output filestream
        //    if (outputFilePath == null)
        //    {
        //        outputFilePath = inputFile;
        //    }

        //    if (!app.Run(inputFile, outputFilePath))
        //    {
        //        return ExitWithError("Unexpected error", logger);
        //    }

        //    return 0;
        //}

    }
}
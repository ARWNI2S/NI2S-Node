using Mono.Options;
using NI2S.Node.Configuration;
using NI2S.Node.Logging;

namespace NI2S.Node
{
    /// <summary>
    /// Represents a NI2S node application.
    /// </summary>
    internal class NI2SNodeApp
    {
        #region Static Class

        private static readonly string ExeName = Path.GetFileName(System.Reflection.Assembly.GetExecutingAssembly().Location);

        /// <summary>
        /// Create new instance of NI2SNodeApp and configure it by parsing arguments.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <param name="logger">Logger (can be null).</param>
        /// <returns>Fully configured NI2SApp.</returns>
        public static NI2SNodeApp CreateNI2SNodeApp(string[] args, TextWriter? logger = null)
        {
            logger ??= Console.Out;
            return CreateNI2SNodeApp(args, logger, out OptionSet p, out bool showHelp, out List<string> extraArgs);
        }

        /// <summary>
        /// Create new instance of NI2SNodeApp and parse arguments.
        /// </summary>
        /// <param name="args">Arguments.</param>
        /// <param name="logger">Logger (can't be null).</param>
        /// <param name="p">Output parsed OptionSet.</param>
        /// <param name="showHelp">Output parsed show help option.</param>
        /// <returns>Newly created NI2SApp with arguments parsed.</returns>
        /// <exception cref="ArgumentNullException">logger is null.</exception>
        public static NI2SNodeApp CreateNI2SNodeApp(string[] args, TextWriter logger, out OptionSet p, out bool showHelp, out List<string> extraArgs)
        {
            if (logger == null) throw new ArgumentNullException(nameof(logger));

            bool localShowHelp = false;

            var app = new NI2SNodeApp(logger);

            p = new OptionSet()
            {
                "Copyrigth (c) 2023 Alternate Reality Worlds. (https://arwni2s.github.io) All Rights Reserved.",
                "Narrative Interactive Intelligent Simulator - Version: "
                +
                string.Format(
                    "{0}.{1}.{2}",
                    typeof(NI2SNodeProgram).Assembly.GetName().Version?.Major,
                    typeof(NI2SNodeProgram).Assembly.GetName().Version?.Minor,
                    typeof(NI2SNodeProgram).Assembly.GetName().Version?.Build) + string.Empty,
                string.Format("Usage: {0} [options]* inputfile -o [outputfile]", ExeName),
                string.Empty,
                "=== Options ===",
                string.Empty,
                { "h|help", "Show this message and exit", v => localShowHelp = v != null },
                { "c|config=", "Configuration file name", v => app._configFilePath = v },
                { "s|sandbox", "Force sandbox mode", v => app.IsSandbox = true },
                { "l|local", "Force local mode", v => app.ForceLocal = true },
                { "debug=", "Set debug log verbosity (defaults off)", v => app.DebugLevel = (DebugLevel)Enum.Parse(typeof(DebugLevel), v) },
                { "name=", "Set a well known name for this node", v => app.NodeName = v },
                //{ "p|platform=", "The platform (Windows, Android, iOS)", v => app.Platform = (PlatformType)Enum.Parse(typeof(PlatformType), v) },
                //{ "t|targetFramework=", "The .NET target platform (platform specific)", v => app.TargetFramework = v },
                //{ "auto-notify-property", "Automatically implements INotifyPropertyChanged", v => app.AutoNotifyProperty = true },
                //{ "parameter-key", "Automatically initialize parameter keys in module static constructor", v => app.ParameterKey = true },
                //{ "rename-assembly=", "Rename assembly", v => app.NewAssemblyName = v },
                //{ "auto-module-initializer", "Execute function tagged with [ModuleInitializer] at module initialization (automatically enabled)", v => app.ModuleInitializer = true },
                //{ "serialization", "Generate serialiation assembly", v => app.SerializationAssembly = true },
                //{ "docfile=", "Generate user documentation from XML file", v => app.DocumentationFile = v },
                //{ "d|directory=", "Additional search directory for assemblies", app.SearchDirectories.Add },
                //{ "a|assembly=", "Additional assembly (for now, it will add the assembly directory to search path)", v => app.SearchDirectories.Add(Path.GetDirectoryName(v)) },
                //{ "signkeyfile=", "Signing Key File", v => app.SignKeyFile = v },
                //{ "references-file=", "Project reference stored in a path", v => app.References.AddRange(File.ReadAllLines(v)) },
                //{ "add-reference=", "References to explicitely add", v => app.ReferencesToAdd.Add(v) },
                //{ "Werror", "Promote warnings to errors", v => app.TreatWarningsAsErrors = true },
                //{ "delete-output-on-error", "Delete output file if an error happened", v => app.DeleteOutputOnError = true },
                //{ "keep-original", "Keep copy of the original assembly", v => app.KeepOriginal = true },
            };

            extraArgs = p.Parse(args);
            showHelp = localShowHelp;

            return app;
        }

        #endregion

        private readonly TextWriter _log;
        private string? _configFilePath = null;
        public string ConfigFile { get { return _configFilePath != null ? _configFilePath : NI2SConfigurationDefaults.AppSettingsFilePath; } }

        public DebugLevel DebugLevel { get; private set; }

        public bool ForceLocal { get; private set; }

        public bool IsSandbox { get; private set; }

        public string? NodeName { get; private set; }


        // Not Creatable.
        private NI2SNodeApp(TextWriter logger) { _log = logger; }

    }
}

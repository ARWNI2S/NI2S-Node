using Mono.Options;
using NI2S.Node.Globalization;

namespace NI2S.Node
{
    /// <summary>
    /// NI2S Node runtime executable program.
    /// </summary>
    public class Program : NodeProgram<Program>
    {
        private OptionSet? _options;

        /// <summary>
        /// Executable entry point.
        /// </summary>
        /// <param name="args">Command line arguments.</param>
        [MTAThread]
        public static int Main(string[] args)
        {
            return ExecuteProgram(args);
        }

        /// <summary>
        /// Default constructor.
        /// </summary>
        public Program() : base() { }

        /// <summary>
        /// Parse arguments into a <see cref="OptionSet"/> instance. This is doing here because Mono.Options nuget package is not needed anywhere.
        /// </summary>
        /// <param name="args"></param>
        protected override void ParseArguments(string[] args)
        {
            _options = new OptionSet()
            {
                StaticStrings.CopyrightNotice,
                StaticStrings.VersionTitle,
                StaticStrings.RuntimeUsage,
                string.Empty,
                StaticStrings.OptionsTitle,
                string.Empty,
                { StaticStrings.HelpSwitch, StaticStrings.HelpSwitchInfo, ProgramOptions.ShowHelpOptionSetter },
                { StaticStrings.DebugSwitch, StaticStrings.DebugSwitchInfo, ProgramOptions.DebugLevelSetter },
                { StaticStrings.VerbositySwitch, StaticStrings.VerbositySwitchInfo, ProgramOptions.LoggerVerbositySetter },
                { StaticStrings.ForceLocalSwitch, StaticStrings.ForceLocalSwitchInfo, ProgramOptions.ForceLocalOptionSetter },
                //{ "o|output=", "Output file name", v => localOutputFilePath = v },
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
            ProgramOptions.ExtraArguments = _options.Parse(args);
        }

        public override void PromptHelp(TextWriter logger)
        {
            logger ??= Console.Out;
            _options?.WriteOptionDescriptions(logger);
        }
    }
}
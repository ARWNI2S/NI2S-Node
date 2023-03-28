using NI2S.Node.Core.Logging;
using NI2S.Node.Diagnostics.Debugger;
using NI2S.Node.Globalization.Resources;
using NI2S.Node.Infrastructure;
using System;

namespace NI2S.Node.Globalization
{
    /// <summary>
    /// Global localized UI strings accessor
    /// </summary>
    public sealed class StaticStrings
    {
        /// <summary>
        /// Localized copyright notice accessor.
        /// </summary>
        public static readonly string CopyrightNotice = LocalizedStrings.Statics_CopyrightNotice_Text;

        /// <summary>
        /// Accessor to a versioning title localized string formatted with actual build version values.
        /// </summary>
        public static readonly string VersionTitle = string.Format(LocalizedStrings.Statics_VersionTitle_Format,
                    typeof(NodeProgram).Assembly.GetName().Version?.Major,
                    typeof(NodeProgram).Assembly.GetName().Version?.Minor,
                    typeof(NodeProgram).Assembly.GetName().Version?.Build,
                    typeof(NodeProgram).Assembly.GetName().Version?.Revision) + string.Empty;

        /// <summary>
        /// Accessor to default usage localized string formatted with actual executable assembly name.
        /// </summary>
        public static readonly string RuntimeUsage = string.Format(LocalizedStrings.Statics_RuntimeUsage_Format, AssemblyGlobals.ExeName);

        /// <summary>
        /// Copyright notice localized string accessor.
        /// </summary>
        public static readonly string OptionsTitle = LocalizedStrings.Statics_OptionsTitle_Text;

        /// <summary>
        /// Constant command line help switch value.
        /// </summary>
        public const string HelpSwitch = "h|help";
        /// <summary>
        /// Localized help switch info string accessor.
        /// </summary>
        public static readonly string HelpSwitchInfo = LocalizedStrings.Statics_HelpSwitchInfo_Text;

        /// <summary>
        /// Constant command line debug switch value.
        /// </summary>
        public const string DebugSwitch = "d|debug=";
        /// <summary>
        /// Localized debug switch info string accessor formatted with actual <see cref="DebugLevel"/> values.
        /// </summary>
        public static readonly string DebugSwitchInfo = string.Format(LocalizedStrings.Statics_DebugSwitchInfo_Format, string.Join(", ", Enum.GetNames<DebugLevel>()));

        /// <summary>
        /// Constant command line logger verbosity switch value.
        /// </summary>
        public const string VerbositySwitch = "verb=";
        /// <summary>
        /// Localized logger verbosity switch info string accessor formatted with actual <see cref="LoggerVerbosity"/> values.
        /// </summary>
        public static readonly string VerbositySwitchInfo = string.Format(LocalizedStrings.Statics_VerbositySwitchInfo_Format, string.Join(", ", Enum.GetNames<LoggerVerbosity>()));

        /// <summary>
        /// Constant command line force local loopback switch value.
        /// </summary>
        public const string ForceLocalSwitch = "force-local";
        /// <summary>
        /// Localized force local loopback switch info string accessor.
        /// </summary>
        public static readonly string ForceLocalSwitchInfo = LocalizedStrings.Statics_ForceLocalSwitchInfo_Text;
    }
}

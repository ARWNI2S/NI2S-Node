using NI2S.Node.Core.Logging;
using NI2S.Node.Diagnostics.Debugger;
using System;
using System.Collections.Generic;

namespace NI2S.Node.Configuration
{
    public class ProgramOptions
    {
        public string[] Args { get; }
        public bool ShowHelp { get; private set; }
        public DebugLevel DebugLevel { get; private set; }
        public LoggerVerbosity LoggerVerbosity { get; private set; }
        public bool ForceLocal { get; private set; }
        public List<string> ExtraArguments { get; set; }

        public ProgramOptions(string[] args)
        {
            Args = args;
        }

        public Action<string> ShowHelpOptionSetter => (v => ShowHelp = v != null);
        public Action<string> DebugLevelSetter => (v => DebugLevel = (DebugLevel)Enum.Parse(typeof(DebugLevel), v));
        public Action<string> LoggerVerbositySetter => (v => LoggerVerbosity = (LoggerVerbosity)Enum.Parse(typeof(LoggerVerbosity), v));
        public Action<string> ForceLocalOptionSetter => (v => ForceLocal = v != null);
    }
}

using ARWNI2S.Infrastructure.Debug;

namespace ARWNI2S.Node.Hosting.Diagnostics
{
    internal class DetailedErrorModel
    {
        public DetailedErrorModel(IEnumerable<ExceptionDetails> errorDetails, bool showRuntimeDetails, string runtimeDisplayName, string runtimeArchitecture, string clrVersion, string currentAssemblyVesion, string operatingSystemDescription)
        {
            ErrorDetails = errorDetails;
            ShowRuntimeDetails = showRuntimeDetails;
            RuntimeDisplayName = runtimeDisplayName;
            RuntimeArchitecture = runtimeArchitecture;
            ClrVersion = clrVersion;
            CurrentAssemblyVesion = currentAssemblyVesion;
            OperatingSystemDescription = operatingSystemDescription;
        }

        /// <summary>
        /// Detailed information about each exception in the stack.
        /// </summary>
        public IEnumerable<ExceptionDetails> ErrorDetails { get; }

        public bool ShowRuntimeDetails { get; }

        public string RuntimeDisplayName { get; }

        public string RuntimeArchitecture { get; }

        public string ClrVersion { get; }

        public string CurrentAssemblyVesion { get; }

        public string OperatingSystemDescription { get; }
    }
}
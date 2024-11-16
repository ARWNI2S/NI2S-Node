using ARWNI2S.Node.Core.Diagnostics.StackTrace;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Runtime.InteropServices;

namespace ARWNI2S.Node.Core.Diagnostics
{
    internal class DetailedErrorModelBuilder
    {
        internal static DetailedErrorModel CreateDetailedErrorModel(IFileProvider contentRootFileProvider, ILogger logger, bool showDetailedErrors, Exception exception)
        {
            var systemRuntimeAssembly = typeof(System.ComponentModel.DefaultValueAttribute).Assembly;
            var assemblyVersion = new AssemblyName(systemRuntimeAssembly.FullName!).Version?.ToString() ?? string.Empty;
            var clrVersion = assemblyVersion;
            var currentAssembly = typeof(DetailedError).Assembly;
            var currentAssemblyVesion = currentAssembly
                .GetCustomAttribute<AssemblyInformationalVersionAttribute>()!
                .InformationalVersion;

            IEnumerable<ExceptionDetails> errorDetails;
            if (showDetailedErrors)
            {
                var exceptionDetailProvider = new ExceptionDetailsProvider(
                    contentRootFileProvider,
                    logger,
                    sourceCodeLineCount: 6);

                errorDetails = exceptionDetailProvider.GetDetails(exception);
            }
            else
            {
                errorDetails = Array.Empty<ExceptionDetails>();
            }

            var model = new DetailedErrorModel(
                errorDetails,
                showDetailedErrors,
                RuntimeInformation.FrameworkDescription,
                RuntimeInformation.ProcessArchitecture.ToString(),
                clrVersion,
                currentAssemblyVesion,
                RuntimeInformation.OSDescription);
            return model;
        }
    }
}

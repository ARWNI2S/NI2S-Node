using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Node.Hosting.Diagnostics
{
    internal class DetailedErrorModelBuilder
    {
        internal static DetailedErrorModel CreateDetailedErrorModel(IFileProvider contentRootFileProvider, ILogger logger, bool showDetailedErrors, Exception exception)
        {
            return new DetailedErrorModel();
        }
    }
}

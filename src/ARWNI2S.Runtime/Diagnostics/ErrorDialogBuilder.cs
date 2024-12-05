using ARWNI2S.Engine;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Diagnostics
{
    internal class ErrorDialogBuilder
    {
        internal static UpdateDelegate BuildErrorPageEngine(IFileProvider contentRootFileProvider, ILogger logger, bool showDetailedErrors, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
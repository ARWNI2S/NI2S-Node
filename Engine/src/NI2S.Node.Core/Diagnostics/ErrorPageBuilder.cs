using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using NI2S.Node.Dummy;
using System;

namespace NI2S.Node.Diagnostics
{
    internal class ErrorPageBuilder
    {
        internal static RequestDelegate BuildErrorPageEngine(IFileProvider contentRootFileProvider, ILogger logger, bool showDetailedErrors, Exception ex)
        {
            throw new NotImplementedException();
        }
    }
}
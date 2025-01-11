// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Node.Diagnostics
{
    internal class ErrorMessageBuilder
    {
        internal static INiisEngine BuildErrorMessageApplication(IFileProvider contentRootFileProvider, ILogger logger, bool showDetailedErrors, Exception ex)
        {
            return null;
        }
    }
}

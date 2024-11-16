using ARWNI2S.Infrastructure;
using ARWNI2S.Infrastructure.Engine;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Logging;
using System.Reflection;

namespace ARWNI2S.Node.Core.Diagnostics
{
    internal class DetailedErrorBuilder
    {
        public static UpdateDelegate BuildEngineErrorFrame(
             IFileProvider contentRootFileProvider,
             ILogger logger,
             bool showDetailedErrors,
             Exception exception)
        {
            if (exception is TargetInvocationException tae)
            {
                exception = tae.InnerException!;
            }

            var model = DetailedErrorModelBuilder.CreateDetailedErrorModel(contentRootFileProvider, logger, showDetailedErrors, exception);

            var errorMessage = new DetailedError(model);
            return context =>
            {
                context.Callback.EventCode = 500;
                context.Callback.CacheControl = EventCacheMode.Disable;
                context.Callback.Tags = "critical,finalize";
                context.Callback.EventTypeId = Constants.EVENTTYPE_ERRORCRITICAL;
                return errorMessage.ExecuteAsync(context);
            };
        }
    }
}
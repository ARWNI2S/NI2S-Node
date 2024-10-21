using ARWNI2S.Infrastructure.Engine;

namespace ARWNI2S.Node.Core.Runtime.Extensions
{
    public static class RuntimeContextExtensions
    {
        public static bool IsSecureContext(this IEngineContext context)
        {
            if (context?.Info == null)
                return true; //Internal always secured... ¿?

            return context.Info.GetClientCertificateAsync() != null;
        }

        public static string GetPathBase(this IEngineContext context)
        {
            if (context.Info == null)
                return string.Empty;

            return string.Empty;
        }
    }
}

namespace ARWNI2S.Node.Core.Runtime.Extensions
{
    public static class RuntimeContextExtensions
    {
        public static bool IsSecureContext(this IRuntimeContext context)
        {
            if (context?.Connection == null)
                return true; //Internal always secured... ¿?

            return context.Connection.GetClientCertificateAsync() != null;
        }

        public static string GetPathBase(this IRuntimeContext context)
        {
            if (context.Connection == null)
                return string.Empty;

            return string.Empty;
        }
    }
}

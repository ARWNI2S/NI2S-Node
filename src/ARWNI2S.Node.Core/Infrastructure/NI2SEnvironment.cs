namespace ARWNI2S.Node.Core.Infrastructure
{
    public class NI2SEnvironment
    {
        //TODO MOVE TO HOST INIT
        public static void SetupEnvironmentVariables()
        {
            // Get ASPNETCORE_ENVIRONMENT if it exists
            string aspnetEnvironment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            // Get NI2S_ENVIRONMENT if it exists
            string ni2sEnvironment = Environment.GetEnvironmentVariable("NI2S_ENVIRONMENT");

            // Detect if we are in a development environment
            bool isDevelopment = aspnetEnvironment == "Development";
#if DEBUG
            if (string.IsNullOrEmpty(aspnetEnvironment))
                isDevelopment = true;
#endif
            // If NI2S_ENVIRONMENT doesn't exist, set it
            if (string.IsNullOrEmpty(ni2sEnvironment))
            {
                // If ASPNETCORE_ENVIRONMENT exists, use its value; otherwise, default to Development or Production
                ni2sEnvironment = string.IsNullOrEmpty(aspnetEnvironment)
                    ? (isDevelopment ? "Development" : "Production")
                    : aspnetEnvironment;

                Environment.SetEnvironmentVariable("NI2S_ENVIRONMENT", ni2sEnvironment);
            }
            else
            {
                // If NI2S_ENVIRONMENT exists, ensure it's equal to ASPNETCORE_ENVIRONMENT if that is defined
                if (!string.IsNullOrEmpty(aspnetEnvironment) && ni2sEnvironment != aspnetEnvironment)
                {
                    ni2sEnvironment = aspnetEnvironment;
                    Environment.SetEnvironmentVariable("NI2S_ENVIRONMENT", ni2sEnvironment);
                }
            }
        }
    }
}

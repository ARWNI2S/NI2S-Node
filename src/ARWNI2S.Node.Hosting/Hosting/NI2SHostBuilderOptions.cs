namespace ARWNI2S.Node.Hosting
{
    //
    // Resumen:
    //     Builder options for use with ConfigureNI2SHost.
    public class NI2SHostBuilderOptions
    {
        public NI2SHostBuilderOptions() { }

        //
        // Resumen:
        //     Indicates if "ASPNETCORE_" prefixed environment variables should be added to
        //     configuration. They are added by default.
        public bool SuppressEnvironmentConfiguration { get; set; }
    }
}

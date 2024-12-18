namespace ARWNI2S.Engine.Configuration
{
    internal class EngineConfig : IConfig
    {
        public string[] ModelValidatorProviders { get; internal set; }
        public string[] ModelMetadataDetailsProviders { get; internal set; }
    }
}
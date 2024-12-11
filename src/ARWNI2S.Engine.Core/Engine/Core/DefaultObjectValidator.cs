using ARWNI2S.Configuration;

namespace ARWNI2S.Engine.Core
{
    internal class DefaultObjectValidator : IObjectModelValidator
    {
        private IModelMetadataProvider modelMetadataProvider;
        private string[] modelValidatorProviders;
        private EngineConfig config;

        public DefaultObjectValidator(IModelMetadataProvider modelMetadataProvider, string[] modelValidatorProviders, EngineConfig config)
        {
            this.modelMetadataProvider = modelMetadataProvider;
            this.modelValidatorProviders = modelValidatorProviders;
            this.config = config;
        }
    }
}
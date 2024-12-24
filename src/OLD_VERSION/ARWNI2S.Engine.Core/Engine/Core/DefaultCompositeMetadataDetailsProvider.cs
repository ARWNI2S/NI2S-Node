namespace ARWNI2S.Engine.Core
{
    internal class DefaultCompositeMetadataDetailsProvider : ICompositeMetadataDetailsProvider
    {
        private string[] modelMetadataDetailsProviders;

        public DefaultCompositeMetadataDetailsProvider(string[] modelMetadataDetailsProviders)
        {
            this.modelMetadataDetailsProviders = modelMetadataDetailsProviders;
        }
    }
}
namespace ARWNI2S.Engine.Parts
{
    public interface IApplicationPartManager
    {
        //public IList<IApplicationFeatureProvider> FeatureProviders { get; } =
        //    new List<IApplicationFeatureProvider>();

        //public IList<ApplicationPart> ApplicationParts { get; } = new List<ApplicationPart>();

        void PopulateFeature<TFeature>(TFeature feature);

        void PopulateDefaultParts(string entryAssemblyName);
    }
}
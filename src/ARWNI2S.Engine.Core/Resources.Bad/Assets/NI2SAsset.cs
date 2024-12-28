using ARWNI2S.Resources;

namespace ARWNI2S.Engine.Resources.Assets
{
    public abstract class NI2SAsset : IResource<AssetInfo>
    {
        public AssetInfo Info { get; }
    }
}

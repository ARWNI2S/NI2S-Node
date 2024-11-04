namespace ARWNI2S.Infrastructure.Assets
{
    public interface IAssetTypeProvider
    {
        bool TryGetContentType(string path, out object var);

    }
}
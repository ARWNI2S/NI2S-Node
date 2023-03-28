using Microsoft.Extensions.Options;

namespace NI2S.Node.Serialization.Configuration
{
    /// <summary>
    /// Provides type manifest information.
    /// </summary>
    public interface ITypeManifestProvider : IConfigureOptions<TypeManifestOptions>
    {
    }
}
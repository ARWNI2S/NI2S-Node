using Microsoft.Extensions.DependencyInjection;

namespace NI2S.Node.Serialization
{
    /// <summary>
    /// Builder interface for configuring serialization.
    /// </summary>
    public interface ISerializerBuilder
    {
        /// <summary>
        /// Gets the service collection.
        /// </summary>
        IServiceCollection Services { get; }
    }
}
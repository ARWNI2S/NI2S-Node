namespace NI2S.Node.Runtime
{
    /// <summary>
    /// Marker interface for addressable endpoints, such as grains, observers, and other system-internal addressable endpoints
    /// </summary>
    [GenerateMethodSerializers(typeof(GrainReference))]
    public interface IAddressable
    {
    }
}

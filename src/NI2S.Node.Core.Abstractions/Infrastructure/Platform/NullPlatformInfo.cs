namespace NI2S.Node.Infrastructure.Platform
{
    public sealed class NullPlatformInfo : PlatformInfo<NullPlatformInfo>
    {
        protected override void ReadCapabilities() { /*DO NOTHING*/ }
    }
}

namespace NI2S.Node.Protocol
{
    public class StringPackageInfo : IKeyedPackageInfo<string>, IStringPackage
    {
        public string Key { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public string[]? Parameters { get; set; }
    }
}
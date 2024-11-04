namespace ARWNI2S.Node.Core.Network
{
    public class HeaderNames
    {
        internal static readonly string Host;
    }

    public class NI2SRequest
    {
        internal readonly Dictionary<string, string> Headers;

        public bool IsSecured { get; internal set; }
        public object PathBase { get; internal set; }
        public string Path { get; internal set; }
    }
}
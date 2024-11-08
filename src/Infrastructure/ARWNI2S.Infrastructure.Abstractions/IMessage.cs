namespace ARWNI2S.Infrastructure
{
    public interface IMessage
    {
        bool IsSecured { get; }
        Dictionary<string, string> Headers { get; }
        object PathBase { get; }
        string Path { get; }
        int StatusCode { get; }
    }
}

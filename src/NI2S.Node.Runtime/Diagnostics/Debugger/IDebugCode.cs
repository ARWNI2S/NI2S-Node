namespace NI2S.Node.Diagnostics.Debugger
{
    internal interface IDebugCode
    {
        DebugLevel Level { get; }
        string Id { get; }
        string Name { get; }
        string Messsage { get; }
    }
}
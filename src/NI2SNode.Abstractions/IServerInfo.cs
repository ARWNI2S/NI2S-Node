namespace NI2S.Node
{
    public interface IServerInfo
    {
        string? Name { get; }

        ServerOptions Options { get; }

        object? DataContext { get; set; }

        int SessionCount { get; }

        IServiceProvider ServiceProvider { get; }

        ServerState State { get; }
    }
}
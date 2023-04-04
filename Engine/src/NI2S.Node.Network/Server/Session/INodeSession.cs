// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network;

namespace NI2S.Node.Network.Server
{
    public enum SystemSessionType
    {
        Cluster,
        Build,
        Remoting,
        User,
    }

    public interface INodeSession : IAppSession
    {
        SystemSessionType SessionType { get; }
    }
}
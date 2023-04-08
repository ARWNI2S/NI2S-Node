// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network.Command;
using NI2S.Node.Network.Protocol;
using System;
using System.Threading.Tasks;

namespace NI2S.Node.Network.Server.Commands
{
    [Command("Play")]
    public class PlayGame : CommandBase, IAsyncCommand<UserSession, DataPackage>
    {
        public ValueTask ExecuteAsync(UserSession session, DataPackage package)
        {
            throw new NotImplementedException();
        }
    }

    [Command("Pause")]
    public class PauseGame : CommandBase, IAsyncCommand<UserSession, DataPackage>
    {
        public ValueTask ExecuteAsync(UserSession session, DataPackage package)
        {
            throw new NotImplementedException();
        }
    }

    [Command("Resume")]
    public class ResumeGame : CommandBase, IAsyncCommand<UserSession, DataPackage>
    {
        public ValueTask ExecuteAsync(UserSession session, DataPackage package)
        {
            throw new NotImplementedException();
        }
    }

    [Command("Stop")]
    public class StopGame : CommandBase, IAsyncCommand<UserSession, DataPackage>
    {
        public ValueTask ExecuteAsync(UserSession session, DataPackage package)
        {
            throw new NotImplementedException();
        }
    }
}

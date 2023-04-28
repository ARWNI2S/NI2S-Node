﻿// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Network.Command;
using NI2S.Node.Network.Protocol;
using System;
using System.Threading.Tasks;

namespace NI2S.Node.Network.Server.Commands
{
    [Command("Build")]
    public class BuildCommand : CommandBase, IAsyncCommand<DeveloperSession, DataPackage>
    {
        public ValueTask ExecuteAsync(DeveloperSession session, DataPackage package)
        {
            throw new NotImplementedException();
        }
    }
}
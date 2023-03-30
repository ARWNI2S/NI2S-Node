using Orleans;
using System;

namespace NI2S.Node.Framework
{
    [GenerateSerializer, Immutable]
    public record class ActorInfo(
        Guid Key,
        string Name);
}

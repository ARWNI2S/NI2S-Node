using Orleans;
using System;

namespace NI2S.Node.Framework
{
    [GenerateSerializer, Immutable]
    public record class CharacterInfo(Guid Key, string Name) : ActorInfo(Key, Name);
}

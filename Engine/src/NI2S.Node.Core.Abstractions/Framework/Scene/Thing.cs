using Orleans;
using System.Collections.Generic;

namespace NI2S.Node.Framework
{
    [GenerateSerializer, Immutable]
    public record class Thing(
        long Id,
        string Name,
        string Category,
        long FoundIn,
        List<string> Commands);
}

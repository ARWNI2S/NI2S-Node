using Orleans;
using System.Threading.Tasks;

namespace NI2S.Node.Framework
{
    /// <summary>
    /// A Actor is, well, there's really no other good name...
    /// </summary>
    public interface IActorGrain : IGrainWithGuidKey
    {
        // Actors have names
        Task<string> Name();
        Task SetName(string name);
    }
}

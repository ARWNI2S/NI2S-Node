using Orleans.Runtime;
using Orleans.Runtime.Placement;

namespace ARWNI2S.Clustering.Nodes
{
    public class NI2SPlacementDirector : IPlacementDirector
    {
        public Task<SiloAddress> OnAddActivation(PlacementStrategy strategy, PlacementTarget target, IPlacementContext context)
        {
            throw new NotImplementedException();
        }
    }
}

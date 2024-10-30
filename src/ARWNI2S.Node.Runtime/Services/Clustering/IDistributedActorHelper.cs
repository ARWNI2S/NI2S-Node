using ARWNI2S.Runtime.Simulation.Actors.Grains;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWNI2S.Runtime.Services.Clustering
{
    public interface IDistributedActorHelper
    {
        INI2SActorGrain GetDistributedActor(Guid uUID);
    }
}

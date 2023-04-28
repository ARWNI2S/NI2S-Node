using Orleans;
using System;
using System.Threading.Tasks;

namespace NI2S.Node.Core.Entities
{
    public interface IEngineClockEntity : IGrainWithStringKey
    {
        Task<DateTime> GetDateTime();
    }
}

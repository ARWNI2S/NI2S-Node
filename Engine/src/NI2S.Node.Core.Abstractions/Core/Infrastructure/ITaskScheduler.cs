// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System.Threading.Tasks;

namespace NI2S.Node.Core.Infrastructure
{
    public interface ITaskScheduler
    {
        Task InitializeAsync();
        void StartScheduler();
    }
}

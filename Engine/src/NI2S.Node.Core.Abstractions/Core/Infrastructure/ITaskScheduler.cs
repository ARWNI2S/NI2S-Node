using System.Threading.Tasks;

namespace NI2S.Node.Core.Infrastructure
{
    public interface ITaskScheduler
    {
        Task InitializeAsync();
        void StartScheduler();
    }
}

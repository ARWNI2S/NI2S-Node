using System.Threading;

namespace NI2S.Node.Dummy
{
    internal interface IDummyRequestLifetimeFeature
    {
        CancellationToken RequestAborted { get; set; }

        void Abort();
    }
}
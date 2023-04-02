using System.Threading;

namespace NI2S.Node.Dummy
{
    internal class DummyRequestLifetimeFeature : IDummyRequestLifetimeFeature
    {
        public CancellationToken RequestAborted { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }

        public void Abort()
        {
            throw new System.NotImplementedException();
        }
    }
}
using System.Security.Claims;

namespace NI2S.Node.Dummy
{
    internal class DummyAuthenticationFeature : IDummyAuthenticationFeature
    {
        public ClaimsPrincipal User { get => throw new System.NotImplementedException(); set => throw new System.NotImplementedException(); }
    }
}
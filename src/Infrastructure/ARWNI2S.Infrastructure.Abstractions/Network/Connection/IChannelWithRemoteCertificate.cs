using System.Security.Cryptography.X509Certificates;

namespace ARWNI2S.Infrastructure.Network.Connection
{
    public interface IConnectionWithRemoteCertificate
    {
        X509Certificate RemoteCertificate { get; }
    }
}

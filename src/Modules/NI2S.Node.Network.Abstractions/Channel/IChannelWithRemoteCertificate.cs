using System.Security.Cryptography.X509Certificates;

namespace NI2S.Node.Networking.Channel
{
    public interface IChannelWithRemoteCertificate
    {
        X509Certificate? RemoteCertificate { get; }
    }
}

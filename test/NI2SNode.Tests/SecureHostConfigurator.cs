using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Client;
using NI2S.Node.Configuration.Options;
using NI2S.Node.Hosting;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using NI2S.Node.Protocol.Security;
using System.Net.Security;
using System.Net.Sockets;
using System.Security.Authentication;

namespace NI2S.Node.Tests
{
    public class SecureHostConfigurator : TcpHostConfigurator
    {
        public SecureHostConfigurator()
        {
            WebSocketSchema = "wss";
            IsSecure = true;
        }

        public override void Configure(INodeHostBuilder hostBuilder)
        {
            hostBuilder.ConfigureServices((ctx, services) =>
            {
                services.Configure<ServerOptions>((options) =>
                {
                    var listener = options.Listeners[0];

                    if (listener.Security == SslProtocols.None)
                        listener.Security = GetServerEnabledSslProtocols();

                    listener.CertificateOptions = new CertificateOptions
                    {
                        FilePath = "arwni2s.pfx",
                        Password = "AK47@ffline"
                    };
                });
            });

            base.Configure(hostBuilder);
        }
        public override async ValueTask<Stream> GetClientStream(Socket socket)
        {
            var stream = new SslStream(new DerivedNetworkStream(socket), false);
            var options = new SslClientAuthenticationOptions();
            options.TargetHost = "arwni2s";
            options.EnabledSslProtocols = GetClientEnabledSslProtocols();
            options.RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true;
            await stream.AuthenticateAsClientAsync(options);
            return stream;
        }

        protected virtual SslProtocols GetServerEnabledSslProtocols()
        {
            return SslProtocols.Tls13 | SslProtocols.Tls12;
        }

        protected virtual SslProtocols GetClientEnabledSslProtocols()
        {
            return SslProtocols.Tls13 | SslProtocols.Tls12;
        }

        public override IEasyClient<TPackageInfo> ConfigureEasyClient<TPackageInfo>(IPipelineFilter<TPackageInfo> pipelineFilter, ChannelOptions options) where TPackageInfo : class
        {
            var client = new EasyClient<TPackageInfo>(pipelineFilter, options);
            client.Security = new SecurityOptions
            {
                TargetHost = "arwni2s",
                EnabledSslProtocols = GetClientEnabledSslProtocols(),
                RemoteCertificateValidationCallback = (sender, certificate, chain, sslPolicyErrors) => true
            };
            return client;
        }
    }

    public class TLS13OnlySecureHostConfigurator : SecureHostConfigurator
    {
        protected override SslProtocols GetServerEnabledSslProtocols()
        {
            return SslProtocols.Tls13;
        }

        protected override SslProtocols GetClientEnabledSslProtocols()
        {
            return SslProtocols.Tls13;
        }
    }
}
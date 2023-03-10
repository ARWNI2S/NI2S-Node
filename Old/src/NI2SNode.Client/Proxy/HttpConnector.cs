﻿using NI2S.Node.Configuration.Options;
using NI2S.Node.Protocol;
using NI2S.Node.Protocol.Channel;
using System.Buffers;
using System.Net;
using System.Text;

namespace NI2S.Node.Client.Proxy
{
    public class HttpConnector : ProxyConnectorBase
    {
        private const string _requestTemplate = "CONNECT {0}:{1} HTTP/1.1\r\nHost: {0}:{1}\r\nProxy-Connection: Keep-Alive\r\n";
        private const string _responsePrefix = "HTTP/1.";
        private const char _space = ' ';
        private readonly string? _username;
        private readonly string? _password;

        public HttpConnector(EndPoint proxyEndPoint)
            : base(proxyEndPoint)
        {

        }

        public HttpConnector(EndPoint proxyEndPoint, string username, string password)
            : this(proxyEndPoint)
        {
            _username = username;
            _password = password;
        }

        protected override async ValueTask<ConnectState?> ConnectProxyAsync(EndPoint remoteEndPoint, ConnectState? state, CancellationToken cancellationToken)
        {
            var encoding = Encoding.ASCII;
            var request = string.Empty;
            var channel = state?.CreateChannel(new LinePipelineFilter(encoding), new ChannelOptions { ReadAsDemand = true });

            channel?.Start();

            if (remoteEndPoint is DnsEndPoint dnsEndPoint)
            {
                request = string.Format(_requestTemplate, dnsEndPoint.Host, dnsEndPoint.Port);
            }
            else if (remoteEndPoint is IPEndPoint ipEndPoint)
            {
                request = string.Format(_requestTemplate, ipEndPoint.Address, ipEndPoint.Port);
            }
            else
            {
                return new ConnectState
                {
                    Result = false,
                    Exception = new Exception($"The endpint type {remoteEndPoint.GetType()} is not supported.")
                };
            }

            // send request
            if (channel != null)
            {
                await channel.SendAsync((writer) =>
                {
                    writer.Write(request, encoding);

                    if (!string.IsNullOrEmpty(_username) || !string.IsNullOrEmpty(_password))
                    {
                        writer.Write("Proxy-Authorization: Basic ", encoding);
                        writer.Write(Convert.ToBase64String(encoding.GetBytes($"{_username}:{_password}")), encoding);
                        writer.Write("\r\n\r\n", encoding);
                    }
                    else
                    {
                        writer.Write("\r\n", encoding);
                    }
                });

                var packStream = channel.GetPackageStream();
                var p = await packStream.ReceiveAsync();

                if (!HandleResponse(p, out string errorMessage))
                {
                    await channel.CloseAsync(CloseReason.ProtocolError);

                    return new ConnectState
                    {
                        Result = false,
                        Exception = new Exception(errorMessage)
                    };
                }

                await channel.DetachAsync();
            }
            return state;
        }

        private static bool HandleResponse(TextPackageInfo p, out string message)
        {
            message = string.Empty;

            if (p == null || string.IsNullOrEmpty(p.Text))
                return false;

            var pos = p.Text.IndexOf(_space);

            // validating response
            if (!p.Text.StartsWith(_responsePrefix, StringComparison.OrdinalIgnoreCase) || pos <= 0)
            {
                message = "Invalid response";
                return false;
            }

            if (!int.TryParse(p.Text.AsSpan().Slice(pos + 1, 3), out var statusCode))
            {
                message = "Invalid response";
                return false;
            }

            if (statusCode < 200 || statusCode > 299)
            {
                message = $"Invalid status code {statusCode}";
                return false;
            }

            return true;
        }
    }
}

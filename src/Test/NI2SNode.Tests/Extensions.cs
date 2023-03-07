using System;
using System.Linq;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Buffers;
using System.Threading.Tasks;
using System.Reflection;
using System.Collections.Generic;
using NI2S.Node;
using NI2S.Node.Command;
using NI2S.Node.Protocol;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Xunit;
using Xunit.Abstractions;
using NI2S.Node.Server;
using System.Threading;
using NI2S.Node.Tests.Command;
using Autofac.Extensions.DependencyInjection;
using Autofac;
using Microsoft.Extensions.Configuration;

namespace NI2S.Node.Tests
{
    public static class Extensions
    {
        internal static IPEndPoint GetServerEndPoint(this IHostConfigurator hostConfigurator)
        {
            return new IPEndPoint(IPAddress.Loopback, hostConfigurator.Listener.Port);
        }
    }
}

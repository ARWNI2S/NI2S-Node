using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NI2S.Node.Infrastructure.Extensions
{
    public static class LoggerExtensions
    {
        public static async Task InformationAsync(this ILogger logger, string message)
        {

        }
        public static async Task InformationAsync(this ILogger logger, string message, params object[] args)
        {

        }
    }
}

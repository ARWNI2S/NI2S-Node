using System.Net;

namespace NI2S.Node
{
    internal static class Extensions
    {
        /// <summary>
        /// 
        /// </summary>
        public static List<Exception> FlattenAggregate(this Exception exc)
        {
            var result = new List<Exception>();
            if (exc is AggregateException && exc.InnerException != null)
                result.AddRange(exc.InnerException.FlattenAggregate());
            else
                result.Add(exc);
            return result;
        }

        /// <summary>
        /// Parse a Uri as an IPEndpoint.
        /// </summary>
        /// <param name="uri">The input Uri</param>
        /// <returns></returns>
        public static IPEndPoint ToIPEndPoint(this Uri uri)
        {
            return uri.Scheme switch
            {
                "gwy.tcp" => new IPEndPoint(IPAddress.Parse(uri.Host), uri.Port),
                //TODO: Exception message: "invalid uri.Scheme"
                _ => throw new InvalidOperationException()
            };
        }

        /// <summary>
        /// Represent an IP end point in the gateway URI format..
        /// </summary>
        /// <param name="ep">The input IP end point</param>
        /// <returns></returns>
        public static Uri ToGatewayUri(this IPEndPoint ep)
        {
            return new Uri(string.Format("gwy.tcp://{0}:{1}/0", ep.Address, ep.Port));
        }
    }
}

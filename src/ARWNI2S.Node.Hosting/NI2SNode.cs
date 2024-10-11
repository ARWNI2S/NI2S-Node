using System.Diagnostics;
using ARWNI2S.Node.Builder;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node
{
    /// <summary>
    /// The NI2S node class used to configure the NI2S simulation and connectivity.
    /// </summary>
    [DebuggerDisplay("{DebuggerToString(),nq}")]
    [DebuggerTypeProxy(typeof(NI2SNodeDebugView))]
    public sealed class NI2SNode : IHost, INodeBuilder, IEndpointRouteBuilder, IAsyncDisposable
    {
        #region Static class

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNode"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SNode"/>.</returns>
        public static NI2SNode Create(string[] args = null) =>
            new NI2SNodeBuilder(new() { Args = args }).Build();

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateBuilder() =>
            new(new());

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with minimal defaults.
        /// </summary>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateMinimalBuilder() =>
            new(new(), minimal: true);

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateBuilder(string[] args) =>
            new(new() { Args = args });

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with minimal defaults.
        /// </summary>
        /// <param name="args">The command line arguments.</param>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateMinimalBuilder(string[] args) =>
            new(new() { Args = args }, minimal: true);

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with preconfigured defaults.
        /// </summary>
        /// <param name="options">The <see cref="NI2SNodeOptions"/> to configure the <see cref="NI2SNodeBuilder"/>.</param>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateBuilder(NI2SNodeOptions options) =>
            new(options);

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with minimal defaults.
        /// </summary>
        /// <param name="options">The <see cref="NI2SNodeOptions"/> to configure the <see cref="NI2SNodeBuilder"/>.</param>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateMinimalBuilder(NI2SNodeOptions options) =>
            new(options, minimal: true);

        /// <summary>
        /// Initializes a new instance of the <see cref="NI2SNodeBuilder"/> class with no defaults.
        /// </summary>
        /// <param name="options">The <see cref="NI2SNodeOptions"/> to configure the <see cref="NI2SNodeBuilder"/>.</param>
        /// <returns>The <see cref="NI2SNodeBuilder"/>.</returns>
        public static NI2SNodeBuilder CreateEmptyBuilder(NI2SNodeOptions options) =>
            new(options, minimal: false, empty: true);

        #endregion

        internal sealed class NI2SNodeDebugView(NI2SNode webApplication)
        {
            private readonly NI2SNode _webApplication = webApplication;

            //public IServiceProvider Services => _webApplication.Services;
            //public IConfiguration Configuration => _webApplication.Configuration;
            //public INI2SHostEnvironment Environment => _webApplication.Environment;
            //public IHostApplicationLifetime Lifetime => _webApplication.Lifetime;
            //public ILogger Logger => _webApplication.Logger;
            //public string Urls => string.Join(", ", _webApplication.Urls);
            //public IReadOnlyList<Endpoint> Endpoints
            //{
            //    get
            //    {
            //        var dataSource = _webApplication.Services.GetRequiredService<EndpointDataSource>();
            //        if (dataSource is CompositeEndpointDataSource compositeEndpointDataSource)
            //        {
            //            // The web app's data sources aren't registered until the routing middleware is. That often happens when the app is run.
            //            // We want endpoints to be available in the debug view before the app starts. Test if all the web app's the data sources are registered.
            //            if (compositeEndpointDataSource.DataSources.Intersect(_webApplication.DataSources).Count() == _webApplication.DataSources.Count)
            //            {
            //                // Data sources are centrally registered.
            //                return dataSource.Endpoints;
            //            }
            //            else
            //            {
            //                // Fallback to just the web app's data sources to support debugging before the web app starts.
            //                return new CompositeEndpointDataSource(_webApplication.DataSources).Endpoints;
            //            }
            //        }

            //        return dataSource.Endpoints;
            //    }
            //}
            //public bool IsRunning => _webApplication.IsRunning;
            //public IList<string> Middleware
            //{
            //    get
            //    {
            //        if (_webApplication.Properties.TryGetValue("__MiddlewareDescriptions", out var value) &&
            //            value is IList<string> descriptions)
            //        {
            //            return descriptions;
            //        }

            //        throw new NotSupportedException("Unable to get configured middleware.");
            //    }
            //}
        }
    }
}
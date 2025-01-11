﻿using ARWNI2S.Configuration;
using Microsoft.Extensions.Configuration;
using System.Diagnostics;

namespace ARWNI2S.Cluster.Configuration
{
    /// <summary>
    /// Configuration loader for Kestrel.
    /// </summary>
    public class ClusterConfigurationLoader
    {
        private readonly IClusterConfigurationService _clusterConfigurationService;

        /// <remarks>
        /// Non-null only makes sense if <see cref="ReloadOnChange"/> is true.
        /// </remarks>
        private readonly CertificatePathWatcher _certificatePathWatcher;

        //private bool _loaded;
        //private bool _endpointsToAddProcessed;

        //// This is not used to trigger reloads but to suppress redundant reloads triggered in other ways
        //private IChangeToken _reloadToken;

        internal ClusterConfigurationLoader(
            ClusterNodeOptions options,
            IConfiguration configuration,
            IClusterConfigurationService clusterConfigurationService,
            CertificatePathWatcher certificatePathWatcher,
            bool reloadOnChange)
        {
            Options = options;
            Configuration = configuration;

            ReloadOnChange = reloadOnChange;

            NodeSettings = Singleton<NI2SSettings>.Instance;

            _clusterConfigurationService = clusterConfigurationService;
            _certificatePathWatcher = certificatePathWatcher;
            Debug.Assert(reloadOnChange || certificatePathWatcher is null, "If reloadOnChange is false, then certificatePathWatcher should be null");
        }

        /// <summary>
        /// Gets the <see cref="ClusterNodeOptions"/>.
        /// </summary>
        public ClusterNodeOptions Options { get; }

        /// <summary>
        /// Gets the application <see cref="IConfiguration"/>.
        /// </summary>
        public IConfiguration Configuration { get; internal set; } // Setter internal for testing

        /// <summary>
        /// If <see langword="true" />, Kestrel will dynamically update endpoint bindings when configuration changes.
        /// This will only reload endpoints defined in the "Endpoints" section of your Kestrel configuration. Endpoints defined in code will not be reloaded.
        /// </summary>
        internal bool ReloadOnChange { get; }

        private NI2SSettings NodeSettings { get; set; }

        //private IDictionary<string, Action<EndpointConfiguration>> EndpointConfigurations { get; }
        //    = new Dictionary<string, Action<EndpointConfiguration>>(0, StringComparer.OrdinalIgnoreCase);

        //// Actions that will be delayed until Load so that they aren't applied if the configuration loader is replaced.
        //private IList<Action> EndpointsToAdd { get; } = new List<Action>();

        //private CertificateConfig? DefaultCertificateConfig { get; set; }
        //internal X509Certificate2? DefaultCertificate { get; set; }

        ///// <summary>
        ///// Specifies a configuration Action to run when an endpoint with the given name is loaded from configuration.
        ///// </summary>
        //public ClusterConfigurationLoader Endpoint(string name, Action<EndpointConfiguration> configureOptions)
        //{
        //    ArgumentException.ThrowIfNullOrEmpty(name);

        //    EndpointConfigurations[name] = configureOptions ?? throw new ArgumentNullException(nameof(configureOptions));
        //    return this;
        //}

        ///// <summary>
        ///// Bind to given IP address and port.
        ///// </summary>
        //public ClusterConfigurationLoader Endpoint(IPAddress address, int port) => Endpoint(address, port, _ => { });

        ///// <summary>
        ///// Bind to given IP address and port.
        ///// </summary>
        //public ClusterConfigurationLoader Endpoint(IPAddress address, int port, Action<ListenOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(address);

        //    return Endpoint(new IPEndPoint(address, port), configure);
        //}

        ///// <summary>
        ///// Bind to given IP endpoint.
        ///// </summary>
        //public ClusterConfigurationLoader Endpoint(IPEndPoint endPoint) => Endpoint(endPoint, _ => { });

        ///// <summary>
        ///// Bind to given IP address and port.
        ///// </summary>
        //public ClusterConfigurationLoader Endpoint(IPEndPoint endPoint, Action<ListenOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(endPoint);
        //    ArgumentNullException.ThrowIfNull(configure);

        //    EndpointsToAdd.Add(() =>
        //    {
        //        Options.Listen(endPoint, configure);
        //    });

        //    return this;
        //}

        ///// <summary>
        ///// Listens on ::1 and 127.0.0.1 with the given port. Requesting a dynamic port by specifying 0 is not supported
        ///// for this type of endpoint.
        ///// </summary>
        //public ClusterConfigurationLoader LocalhostEndpoint(int port) => LocalhostEndpoint(port, options => { });

        ///// <summary>
        ///// Listens on ::1 and 127.0.0.1 with the given port. Requesting a dynamic port by specifying 0 is not supported
        ///// for this type of endpoint.
        ///// </summary>
        //public ClusterConfigurationLoader LocalhostEndpoint(int port, Action<ListenOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(configure);

        //    EndpointsToAdd.Add(() =>
        //    {
        //        Options.ListenLocalhost(port, configure);
        //    });

        //    return this;
        //}

        ///// <summary>
        ///// Listens on all IPs using IPv6 [::], or IPv4 0.0.0.0 if IPv6 is not supported.
        ///// </summary>
        //public ClusterConfigurationLoader AnyIPEndpoint(int port) => AnyIPEndpoint(port, options => { });

        ///// <summary>
        ///// Listens on all IPs using IPv6 [::], or IPv4 0.0.0.0 if IPv6 is not supported.
        ///// </summary>
        //public ClusterConfigurationLoader AnyIPEndpoint(int port, Action<ListenOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(configure);

        //    EndpointsToAdd.Add(() =>
        //    {
        //        Options.ListenAnyIP(port, configure);
        //    });

        //    return this;
        //}

        ///// <summary>
        ///// Bind to given Unix domain socket path.
        ///// </summary>
        //public ClusterConfigurationLoader UnixSocketEndpoint(string socketPath) => UnixSocketEndpoint(socketPath, _ => { });

        ///// <summary>
        ///// Bind to given Unix domain socket path.
        ///// </summary>
        //public ClusterConfigurationLoader UnixSocketEndpoint(string socketPath, Action<ListenOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(socketPath);
        //    if (socketPath.Length == 0 || socketPath[0] != '/')
        //    {
        //        throw new ArgumentException(CoreStrings.UnixSocketPathMustBeAbsolute, nameof(socketPath));
        //    }
        //    ArgumentNullException.ThrowIfNull(configure);

        //    EndpointsToAdd.Add(() =>
        //    {
        //        Options.ListenUnixSocket(socketPath, configure);
        //    });

        //    return this;
        //}

        ///// <summary>
        ///// Bind to given named pipe.
        ///// </summary>
        //public ClusterConfigurationLoader NamedPipeEndpoint(string pipeName) => NamedPipeEndpoint(pipeName, _ => { });

        ///// <summary>
        ///// Bind to given named pipe.
        ///// </summary>
        //public ClusterConfigurationLoader NamedPipeEndpoint(string pipeName, Action<ListenOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(pipeName);
        //    ArgumentNullException.ThrowIfNull(configure);

        //    EndpointsToAdd.Add(() =>
        //    {
        //        Options.ListenNamedPipe(pipeName, configure);
        //    });

        //    return this;
        //}

        ///// <summary>
        ///// Open a socket file descriptor.
        ///// </summary>
        //public ClusterConfigurationLoader HandleEndpoint(ulong handle) => HandleEndpoint(handle, _ => { });

        ///// <summary>
        ///// Open a socket file descriptor.
        ///// </summary>
        //public ClusterConfigurationLoader HandleEndpoint(ulong handle, Action<ListenOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(configure);

        //    EndpointsToAdd.Add(() =>
        //    {
        //        Options.ListenHandle(handle, configure);
        //    });

        //    return this;
        //}

        //// Called from ClusterNodeOptions.ApplyEndpointDefaults so it applies to even explicit Listen endpoints.
        //// Does not require a call to Load.
        //internal void ApplyEndpointDefaults(ListenOptions listenOptions)
        //{
        //    var defaults = ConfigurationReader.EndpointDefaults;

        //    if (defaults.Protocols.HasValue)
        //    {
        //        listenOptions.Protocols = defaults.Protocols.Value;
        //    }
        //}

        //// Called from ClusterNodeOptions.ApplyHttpsDefaults so it applies to even explicit Listen endpoints.
        //// Does not require a call to Load.
        //internal void ApplyHttpsDefaults(HttpsConnectionAdapterOptions httpsOptions)
        //{
        //    var defaults = ConfigurationReader.EndpointDefaults;

        //    if (defaults.SslProtocols.HasValue)
        //    {
        //        httpsOptions.SslProtocols = defaults.SslProtocols.Value;
        //    }

        //    if (defaults.ClientCertificateMode.HasValue)
        //    {
        //        httpsOptions.ClientCertificateMode = defaults.ClientCertificateMode.Value;
        //    }
        //}

        //// Note: This method is obsolete, but we have to keep it around to avoid breaking the public API.
        //// Internally, we should always use <see cref="LoadInternal"/>.
        ///// <summary>
        ///// Loads the configuration.  Does nothing if it has previously been invoked (including implicitly).
        ///// </summary>
        //public void Load()
        //{
        //    if (!_loaded)
        //    {
        //        LoadInternal();
        //    }

        //    // Has its own logic for skipping subsequent invocations
        //    ProcessEndpointsToAdd();
        //}

        ///// <remarks>
        ///// Always prefer this to <see cref="Load"/> since it can be called repeatedly and no-ops if
        ///// there's a change token indicating nothing has changed.
        ///// </remarks>
        //internal void LoadInternal()
        //{
        //    if (!_loaded || ReloadOnChange)
        //    {
        //        Debug.Assert(!!_loaded || _reloadToken is null, "Shouldn't have a reload token before first load");
        //        Debug.Assert(!!ReloadOnChange || _reloadToken is null, "Shouldn't have a reload token unless reload-on-change is set");

        //        _loaded = true;

        //        if (_reloadToken is null || _reloadToken.HasChanged)
        //        {
        //            // Will update _reloadToken
        //            _ = Reload();
        //        }
        //    }
        //}

        //internal void ProcessEndpointsToAdd()
        //{
        //    if (_endpointsToAddProcessed)
        //    {
        //        return;
        //    }
        //    // Set this *before* invoking delegates, in case one throws
        //    _endpointsToAddProcessed = true;

        //    foreach (var action in EndpointsToAdd)
        //    {
        //        action();
        //    }
        //}

        //internal IChangeToken? GetReloadToken()
        //{
        //    Debug.Assert(ReloadOnChange);

        //    var configToken = Configuration.GetReloadToken();

        //    if (_certificatePathWatcher is null)
        //    {
        //        return configToken;
        //    }

        //    var watcherToken = _certificatePathWatcher.GetChangeToken();
        //    return new CompositeChangeToken(new[] { configToken, watcherToken });
        //}

        //// Adds endpoints from config to ClusterNodeOptions.ConfigurationBackedListenOptions and configures some other options.
        //// Any endpoints that were removed from the last time endpoints were loaded are returned.
        //internal (List<ListenOptions>, List<ListenOptions>) Reload()
        //{
        //    if (ReloadOnChange)
        //    {
        //        _reloadToken = GetReloadToken();
        //    }

        //    var endpointsToStop = Options.ConfigurationBackedListenOptions.ToList();
        //    var endpointsToStart = new List<ListenOptions>();
        //    var endpointsToReuse = new List<ListenOptions>();

        //    var oldDefaultCertificateConfig = DefaultCertificateConfig;

        //    DefaultCertificateConfig = null;
        //    DefaultCertificate = null;

        //    ConfigurationReader = new ConfigurationReader(Configuration);

        //    if (_clusterConfigurationService.IsInitialized && _clusterConfigurationService.LoadDefaultCertificate(ConfigurationReader) is CertificateAndConfig certPair)
        //    {
        //        DefaultCertificate = certPair.Certificate;
        //        DefaultCertificateConfig = certPair.CertificateConfig;
        //    }

        //    foreach (var endpoint in ConfigurationReader.Endpoints)
        //    {
        //        var listenOptions = AddressBinder.ParseAddress(endpoint.Url, out var https);

        //        if (!https)
        //        {
        //            ConfigurationReader.ThrowIfContainsHttpsOnlyConfiguration(endpoint);
        //        }

        //        Options.ApplyEndpointDefaults(listenOptions);

        //        if (endpoint.Protocols.HasValue)
        //        {
        //            listenOptions.Protocols = endpoint.Protocols.Value;
        //        }
        //        else
        //        {
        //            // Ensure endpoint is reloaded if it used the default protocol and the protocol changed.
        //            // listenOptions.Protocols should already be set to this by ApplyEndpointDefaults.
        //            endpoint.Protocols = ConfigurationReader.EndpointDefaults.Protocols;
        //        }

        //        // Compare to UseHttps(httpsOptions => { })
        //        var httpsOptions = new HttpsConnectionAdapterOptions();

        //        if (https)
        //        {
        //            // Throws an appropriate exception if https configuration isn't enabled
        //            _clusterConfigurationService.ApplyHttpsConfiguration(httpsOptions, endpoint, Options, DefaultCertificateConfig, ConfigurationReader);
        //        }

        //        // Now that defaults have been loaded, we can compare to the currently bound endpoints to see if the config changed.
        //        // There's no reason to rerun an EndpointConfigurations callback if nothing changed.
        //        var matchingBoundEndpoints = new List<ListenOptions>();
        //        foreach (var o in endpointsToStop)
        //        {
        //            if (o.EndpointConfig == endpoint)
        //            {
        //                Debug.Assert(o.EndpointConfig?.Certificate?.FileHasChanged != true, "Preserving an endpoint with file changes");
        //                matchingBoundEndpoints.Add(o);
        //            }
        //        }

        //        if (matchingBoundEndpoints.Count > 0)
        //        {
        //            endpointsToStop.RemoveAll(o => o.EndpointConfig == endpoint);
        //            endpointsToReuse.AddRange(matchingBoundEndpoints);
        //            continue;
        //        }

        //        if (EndpointConfigurations.TryGetValue(endpoint.Name, out var configureEndpoint))
        //        {
        //            var endpointConfig = new EndpointConfiguration(https, listenOptions, httpsOptions, endpoint.ConfigSection);
        //            configureEndpoint(endpointConfig);
        //        }

        //        // EndpointDefaults or configureEndpoint may have added an https adapter.
        //        if (https)
        //        {
        //            // This would throw if it were invoked without https configuration having been enabled,
        //            // but that won't happen because ApplyHttpsConfiguration would throw above under those
        //            // circumstances.
        //            _clusterConfigurationService.UseHttpsWithSni(listenOptions, httpsOptions, endpoint);
        //        }

        //        listenOptions.EndpointConfig = endpoint;

        //        endpointsToStart.Add(listenOptions);
        //    }

        //    // Update ConfigurationBackedListenOptions after everything else has been processed so that
        //    // it's left in a good state (i.e. its former state) if something above throws an exception.
        //    // Note that this isn't foolproof - we could run out of memory or something - but it covers
        //    // exceptions resulting from user misconfiguration (e.g. bad endpoint cert password).
        //    Options.ConfigurationBackedListenOptions.Clear();
        //    Options.ConfigurationBackedListenOptions.AddRange(endpointsToReuse);
        //    Options.ConfigurationBackedListenOptions.AddRange(endpointsToStart);

        //    if (ReloadOnChange && _certificatePathWatcher is not null)
        //    {
        //        var certificateConfigsToRemove = new List<CertificateConfig>();
        //        var certificateConfigsToAdd = new List<CertificateConfig>();

        //        if (DefaultCertificateConfig != oldDefaultCertificateConfig)
        //        {
        //            if (DefaultCertificateConfig?.IsFileCert == true)
        //            {
        //                certificateConfigsToAdd.Add(DefaultCertificateConfig);
        //            }

        //            if (oldDefaultCertificateConfig is not null)
        //            {
        //                certificateConfigsToRemove.Add(oldDefaultCertificateConfig);
        //            }
        //        }

        //        foreach (var endpointToStart in endpointsToStart)
        //        {
        //            var endpointConfig = endpointToStart.EndpointConfig;
        //            if (endpointConfig is null)
        //            {
        //                continue;
        //            }

        //            var certConfig = endpointConfig.Certificate;
        //            if (certConfig?.IsFileCert == true)
        //            {
        //                certificateConfigsToAdd.Add(certConfig);
        //            }

        //            foreach (var sniConfig in endpointConfig.Sni.Values)
        //            {
        //                var sniCertConfig = sniConfig.Certificate;
        //                if (sniCertConfig?.IsFileCert == true)
        //                {
        //                    certificateConfigsToAdd.Add(sniCertConfig);
        //                }
        //            }
        //        }

        //        foreach (var endpointToStop in endpointsToStop)
        //        {
        //            var endpointConfig = endpointToStop.EndpointConfig;
        //            if (endpointConfig is null)
        //            {
        //                continue;
        //            }

        //            var certConfig = endpointConfig.Certificate;
        //            if (certConfig?.IsFileCert == true)
        //            {
        //                certificateConfigsToRemove.Add(certConfig);
        //            }

        //            foreach (var sniConfig in endpointConfig.Sni.Values)
        //            {
        //                var sniCertConfig = sniConfig.Certificate;
        //                if (sniCertConfig?.IsFileCert == true)
        //                {
        //                    certificateConfigsToRemove.Add(sniCertConfig);
        //                }
        //            }
        //        }

        //        _certificatePathWatcher.UpdateWatches(certificateConfigsToRemove, certificateConfigsToAdd);
        //    }

        //    return (endpointsToStop, endpointsToStart);
        //}
    }
}

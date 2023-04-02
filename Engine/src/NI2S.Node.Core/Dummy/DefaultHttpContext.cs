using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace NI2S.Node.Dummy
{
    /// <summary>
    /// Represents an implementation of the HTTP Context class.
    /// </summary>
    public sealed class DefaultDummyContext : DummyContext
    {
        // The initial size of the feature collection when using the default constructor; based on number of common features
        // https://github.com/dotnet/aspnetcore/issues/31249
        private const int DefaultFeatureCollectionSize = 10;

        // Lambdas hoisted to static readonly fields to improve inlining https://github.com/dotnet/roslyn/issues/13624
        private static readonly Func<IFeatureCollection, IItemsFeature> _newItemsFeature = f => new ItemsFeature();
        private static readonly Func<DefaultDummyContext, IServiceProvidersFeature> _newServiceProvidersFeature = context => new RequestServicesFeature(context, context.ServiceScopeFactory);
        private static readonly Func<IFeatureCollection, IDummyAuthenticationFeature> _newDummyAuthenticationFeature = f => new DummyAuthenticationFeature();
        private static readonly Func<IFeatureCollection, IDummyRequestLifetimeFeature> _newDummyRequestLifetimeFeature = f => new DummyRequestLifetimeFeature();
        private static readonly Func<IFeatureCollection, ISessionFeature> _newSessionFeature = f => new DefaultSessionFeature();
        private static readonly Func<IFeatureCollection, ISessionFeature> _nullSessionFeature = f => null;
        private static readonly Func<IFeatureCollection, IDummyRequestIdentifierFeature> _newDummyRequestIdentifierFeature = f => new DummyRequestIdentifierFeature();

        private FeatureReferences<FeatureInterfaces> _features;

        private readonly DefaultDummyRequest _request;
        private readonly DefaultDummyResponse _response;

        private DefaultConnectionInfo _connection;
        private DefaultWebSocketManager _websockets;

        // This is field exists to make analyzing memory dumps easier.
        // https://github.com/dotnet/aspnetcore/issues/29709
        internal bool _active;

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDummyContext"/> class.
        /// </summary>
        public DefaultDummyContext()
            : this(new FeatureCollection(DefaultFeatureCollectionSize))
        {
            Features.Set<IDummyRequestFeature>(new DummyRequestFeature());
            Features.Set<IDummyResponseFeature>(new DummyResponseFeature());
            Features.Set<IDummyResponseBodyFeature>(new StreamResponseBodyFeature(Stream.Null));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DefaultDummyContext"/> class with provided features.
        /// </summary>
        /// <param name="features">Initial set of features for the <see cref="DefaultDummyContext"/>.</param>
        public DefaultDummyContext(IFeatureCollection features)
        {
            _features.Initalize(features);
            _request = new DefaultDummyRequest(this);
            _response = new DefaultDummyResponse(this);
        }

        /// <summary>
        /// Reinitialize  the current instant of the class with features passed in.
        /// </summary>
        /// <remarks>
        /// This method allows the consumer to re-use the <see cref="DefaultDummyContext" /> for another request, rather than having to allocate a new instance.
        /// </remarks>
        /// <param name="features">The new set of features for the <see cref="DefaultDummyContext" />.</param>
        public void Initialize(IFeatureCollection features)
        {
            var revision = features.Revision;
            _features.Initalize(features, revision);
            _request.Initialize(revision);
            _response.Initialize(revision);
            _connection?.Initialize(features, revision);
            _websockets?.Initialize(features, revision);
            _active = true;
        }

        /// <summary>
        /// Uninitialize all the features in the <see cref="DefaultDummyContext" />.
        /// </summary>
        public void Uninitialize()
        {
            _features = default;
            _request.Uninitialize();
            _response.Uninitialize();
            _connection?.Uninitialize();
            _websockets?.Uninitialize();
            _active = false;
        }

        /// <summary>
        /// Gets or set the <see cref="FormOptions" /> for this instance.
        /// </summary>
        /// <returns>
        /// <see cref="FormOptions"/>
        /// </returns>
        public FormOptions FormOptions { get; set; } = default!;

        /// <summary>
        /// Gets or sets the <see cref="IServiceScopeFactory" /> for this instance.
        /// </summary>
        /// <returns>
        /// <see cref="IServiceScopeFactory"/>
        /// </returns>
        public IServiceScopeFactory ServiceScopeFactory { get; set; } = default!;

        private IItemsFeature ItemsFeature =>
            _features.Fetch(ref _features.Cache.Items, _newItemsFeature)!;

        private IServiceProvidersFeature ServiceProvidersFeature =>
            _features.Fetch(ref _features.Cache.ServiceProviders, this, _newServiceProvidersFeature)!;

        private IDummyAuthenticationFeature DummyAuthenticationFeature =>
            _features.Fetch(ref _features.Cache.Authentication, _newDummyAuthenticationFeature)!;

        private IDummyRequestLifetimeFeature LifetimeFeature =>
            _features.Fetch(ref _features.Cache.Lifetime, _newDummyRequestLifetimeFeature)!;

        private ISessionFeature SessionFeature =>
            _features.Fetch(ref _features.Cache.Session, _newSessionFeature)!;

        private ISessionFeature SessionFeatureOrNull =>
            _features.Fetch(ref _features.Cache.Session, _nullSessionFeature);

        private IDummyRequestIdentifierFeature RequestIdentifierFeature =>
            _features.Fetch(ref _features.Cache.RequestIdentifier, _newDummyRequestIdentifierFeature)!;

        /// <inheritdoc/>
        public override IFeatureCollection Features => _features.Collection ?? ContextDisposed();

        /// <inheritdoc/>
        public override DummyRequest Request => _request;

        /// <inheritdoc/>
        public override DummyResponse Response => _response;

        /// <inheritdoc/>
        public override ConnectionInfo Connection => _connection ?? (_connection = new DefaultConnectionInfo(Features));

        /// <inheritdoc/>
        public override WebSocketManager WebSockets => _websockets ?? (_websockets = new DefaultWebSocketManager(Features));

        /// <inheritdoc/>
        public override ClaimsPrincipal User
        {
            get
            {
                var user = DummyAuthenticationFeature.User;
                if (user == null)
                {
                    user = new ClaimsPrincipal(new ClaimsIdentity());
                    DummyAuthenticationFeature.User = user;
                }
                return user;
            }
            set { DummyAuthenticationFeature.User = value; }
        }

        /// <inheritdoc/>
        public override IDictionary<object, object> Items
        {
            get { return ItemsFeature.Items; }
            set { ItemsFeature.Items = value; }
        }

        /// <inheritdoc/>
        public override IServiceProvider RequestServices
        {
            get { return ServiceProvidersFeature.RequestServices; }
            set { ServiceProvidersFeature.RequestServices = value; }
        }

        /// <inheritdoc/>
        public override CancellationToken RequestAborted
        {
            get { return LifetimeFeature.RequestAborted; }
            set { LifetimeFeature.RequestAborted = value; }
        }

        /// <inheritdoc/>
        public override string TraceIdentifier
        {
            get { return RequestIdentifierFeature.TraceIdentifier; }
            set { RequestIdentifierFeature.TraceIdentifier = value; }
        }

        /// <inheritdoc/>
        public override ISession Session
        {
            get
            {
                var feature = SessionFeatureOrNull;
                if (feature == null)
                {
                    throw new InvalidOperationException("Session has not been configured for this application " +
                        "or request.");
                }
                return feature.Session;
            }
            set
            {
                SessionFeature.Session = value;
            }
        }

        // This property exists because of backwards compatibility.
        // We send an anonymous object with an DummyContext property
        // via DiagnosticListener in various events throughout the pipeline. Instead
        // we just send the DummyContext to avoid extra allocations
        /// <summary>
        /// This API is used by ASP.NET Core's infrastructure and should not be used by application code.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public DummyContext DummyContext => this;

        /// <inheritdoc/>
        public override void Abort()
        {
            LifetimeFeature.Abort();
        }

        private static IFeatureCollection ContextDisposed()
        {
            ThrowContextDisposed();
            return null;
        }

        [DoesNotReturn]
        private static void ThrowContextDisposed()
        {
            throw new ObjectDisposedException(nameof(DummyContext), $"Request has finished and {nameof(DummyContext)} disposed.");
        }

        struct FeatureInterfaces
        {
            public IItemsFeature Items;
            public IServiceProvidersFeature ServiceProviders;
            public IDummyAuthenticationFeature Authentication;
            public IDummyRequestLifetimeFeature Lifetime;
            public ISessionFeature Session;
            public IDummyRequestIdentifierFeature RequestIdentifier;
        }
    }
}

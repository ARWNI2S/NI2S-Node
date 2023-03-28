using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Dummy;
using NI2S.Node.Engine.Modules;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace NI2S.Node.Hosting.Server
{
    /// <summary>
    /// A factory for creating <see cref="DummyContext" /> instances.
    /// </summary>
    public class DefaultDummyContextFactory : IDummyContextFactory
    {
        private readonly IDummyContextAccessor _dummyContextAccessor;
        //private readonly FormOptions _formOptions;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        // This takes the IServiceProvider because it needs to support an ever expanding
        // set of services that flow down into DummyContext features
        /// <summary>
        /// Creates a factory for creating <see cref="DummyContext" /> instances.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> to be used when retrieving services.</param>
        public DefaultDummyContextFactory(IServiceProvider serviceProvider)
        {
            // May be null
            _dummyContextAccessor = serviceProvider.GetService<IDummyContextAccessor>();
            //_formOptions = serviceProvider.GetRequiredService<IOptions<FormOptions>>().Value;
            _serviceScopeFactory = serviceProvider.GetRequiredService<IServiceScopeFactory>();
        }

        internal IDummyContextAccessor DummyContextAccessor => _dummyContextAccessor;

        /// <summary>
        /// Create an <see cref="DummyContext"/> instance given an <paramref name="featureCollection" />.
        /// </summary>
        /// <param name="featureCollection"></param>
        /// <returns>An initialized <see cref="DummyContext"/> object.</returns>
        public DummyContext Create(IDummyFeatureCollection featureCollection)
        {
            ArgumentNullException.ThrowIfNull(featureCollection);

            var dummyContext = new DefaultDummyContext(featureCollection);
            Initialize(dummyContext, featureCollection);
            return dummyContext;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal void Initialize(DefaultDummyContext dummyContext, IDummyFeatureCollection featureCollection)
        {
            Debug.Assert(featureCollection != null);
            Debug.Assert(dummyContext != null);

            //dummyContext.Initialize(featureCollection);

            if (_dummyContextAccessor != null)
            {
                _dummyContextAccessor.DummyContext = dummyContext;
            }

            //dummyContext.FormOptions = _formOptions;
            //dummyContext.ServiceScopeFactory = _serviceScopeFactory;
        }

        /// <summary>
        /// Clears the current <see cref="DummyContext" />.
        /// </summary>
        public void Dispose(DummyContext dummyContext)
        {
            if (_dummyContextAccessor != null)
            {
                _dummyContextAccessor.DummyContext = null;
            }
        }

        internal void Dispose(DefaultDummyContext dummyContext)
        {
            if (_dummyContextAccessor != null)
            {
                _dummyContextAccessor.DummyContext = null;
            }

            //dummyContext.Uninitialize();
        }
    }
}

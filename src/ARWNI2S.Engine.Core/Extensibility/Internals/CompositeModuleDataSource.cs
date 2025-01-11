// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Extensibility;
using Microsoft.Extensions.Primitives;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics;

namespace ARWNI2S.Engine.Extensibility.Internals
{
    /// <summary>
    /// Represents an <see cref="ModuleDataSource"/> whose values come from a collection of <see cref="ModuleDataSource"/> instances.
    /// </summary>
    [DebuggerDisplay("{DebuggerDisplayString,nq}")]
    internal sealed class CompositeModuleDataSource : ModuleDataSource, IDisposable
    {
#if NET9_0_OR_GREATER
        private readonly Lock _lock = new();
#else
        private readonly object _lock = new();
#endif
        private readonly ObservableCollection<IModuleDataSource> _dataSources;

        private List<IModule> _modules;
        private IChangeToken _consumerChangeToken;
        private CancellationTokenSource _cts;
        private List<IDisposable> _changeTokenRegistrations;
        private bool _disposed;

        internal CompositeModuleDataSource(ObservableCollection<IModuleDataSource> dataSources)
        {
            dataSources.CollectionChanged += OnDataSourcesChanged;
            _dataSources = dataSources;
        }

        /// <summary>
        /// Instantiates a <see cref="CompositeModuleDataSource"/> object from <paramref name="moduleDataSources"/>.
        /// </summary>
        /// <param name="moduleDataSources">An collection of <see cref="IModuleDataSource" /> objects.</param>
        /// <returns>A <see cref="CompositeModuleDataSource"/>.</returns>
        public CompositeModuleDataSource(IEnumerable<IModuleDataSource> moduleDataSources)
        {
            _dataSources = [.. moduleDataSources];
        }

        private void OnDataSourcesChanged(object sender, NotifyCollectionChangedEventArgs e) => HandleChange(collectionChanged: true);

        /// <summary>
        /// Returns the collection of <see cref="IModuleDataSource"/> instances associated with the object.
        /// </summary>
        public IEnumerable<IModuleDataSource> DataSources => _dataSources;

        /// <summary>
        /// Gets a <see cref="IChangeToken"/> used to signal invalidation of cached <see cref="IModule"/> instances.
        /// </summary>
        /// <returns>The <see cref="IChangeToken"/>.</returns>
        public override IChangeToken GetChangeToken()
        {
            EnsureChangeTokenInitialized();
            return _consumerChangeToken;
        }

        /// <summary>
        /// Returns a read-only collection of <see cref="IModule"/> instances.
        /// </summary>
        public override IReadOnlyList<IModule> Modules
        {
            get
            {
                EnsureModulesInitialized();
                return _modules;
            }
        }

        /// <inheritdoc/>
        public override IReadOnlyList<IModule> GetGroupedModules(ModuleGroupContext context)
        {
            if (_dataSources.Count is 0)
            {
                return [];
            }

            // We could try to optimize the single data source case by returning its result directly like GroupDataSource does,
            // but the CompositeModuleDataSourceTest class was picky about the Modules property creating a shallow copy,
            // so we'll shallow copy here for consistency.
            var groupedModules = new List<IModule>();

            foreach (var dataSource in _dataSources)
            {
                groupedModules.AddRange(dataSource.GetGroupedModules(context));
            }

            // There's no need to cache these the way we do with _modules. This is only ever used to get intermediate results.
            // Anything using the DataSourceDependentCache like the DfaMatcher will resolve the cached Modules property.
            return groupedModules;
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            // CompositeDataSource is registered as a singleton by default by AddRouting().
            // UseModules() adds all root data sources to this singleton.
            List<IDisposable> disposables = null;

            lock (_lock)
            {
                _disposed = true;

                if (_dataSources is ObservableCollection<IModuleDataSource> observableDataSources)
                {
                    observableDataSources.CollectionChanged -= OnDataSourcesChanged;
                }

                foreach (var dataSource in _dataSources)
                {
                    if (dataSource is IDisposable disposableDataSource)
                    {
                        disposables ??= [];
                        disposables.Add(disposableDataSource);
                    }
                }

                if (_changeTokenRegistrations is { Count: > 0 })
                {
                    disposables ??= [];
                    disposables.AddRange(_changeTokenRegistrations);
                }
            }

            // Dispose everything outside of the lock in case a registration is blocking on HandleChange completing
            // on another thread or something.
            if (disposables is not null)
            {
                foreach (var disposable in disposables)
                {
                    disposable.Dispose();
                }
            }
        }

        // Defer initialization to avoid doing lots of reflection on startup.
        [MemberNotNull(nameof(_modules))]
        private void EnsureModulesInitialized()
        {
            if (_modules is not null)
            {
                return;
            }

            lock (_lock)
            {
                if (_modules is not null)
                {
                    return;
                }

                // Now that we're caching the _enpoints, we're responsible for keeping them up-to-date even if the caller
                // hasn't started listening for changes themselves yet.
                EnsureChangeTokenInitialized();

                // Note: we can't use DataSourceDependentCache here because we also need to handle a list of change
                // tokens, which is a complication most of our code doesn't have.
                CreateModulesUnsynchronized();
            }
        }

        [MemberNotNull(nameof(_consumerChangeToken))]
        private void EnsureChangeTokenInitialized()
        {
            if (_consumerChangeToken is not null)
            {
                return;
            }

            lock (_lock)
            {
                if (_consumerChangeToken is not null)
                {
                    return;
                }

                // This is our first time initializing the change token, so the collection has "changed" from nothing.
                CreateChangeTokenUnsynchronized(collectionChanged: true);
            }
        }

        private void HandleChange(bool collectionChanged)
        {
            CancellationTokenSource oldTokenSource = null;
            List<IDisposable> oldChangeTokenRegistrations = null;

            lock (_lock)
            {
                if (_disposed)
                {
                    return;
                }

                // Prevent consumers from re-registering callback to in-flight events as that can
                // cause a stack overflow.
                // Example:
                // 1. B registers A.
                // 2. A fires event causing B's callback to get called.
                // 3. B executes some code in its callback, but needs to re-register callback
                //    in the same callback.
                oldTokenSource = _cts;
                oldChangeTokenRegistrations = _changeTokenRegistrations;

                // Don't create a new change token if no one is listening.
                if (oldTokenSource is not null)
                {
                    // We have to hook to any OnChange callbacks before caching modules,
                    // otherwise we might miss changes that occurred to one of the _dataSources after caching.
                    CreateChangeTokenUnsynchronized(collectionChanged);
                }

                // Don't update modules if no one has read them yet.
                if (_modules is not null)
                {
                    // Refresh the modules from data source so that callbacks can get the latest modules.
                    CreateModulesUnsynchronized();
                }
            }

            // Disposing registrations can block on user defined code on running on other threads that could try to acquire the _lock.
            if (collectionChanged && oldChangeTokenRegistrations is not null)
            {
                foreach (var registration in oldChangeTokenRegistrations)
                {
                    registration.Dispose();
                }
            }

            // Raise consumer callbacks. Any new callback registration would happen on the new token created in earlier step.
            // Avoid raising callbacks inside a lock.
            oldTokenSource?.Cancel();
        }

        [MemberNotNull(nameof(_consumerChangeToken))]
        private void CreateChangeTokenUnsynchronized(bool collectionChanged)
        {
            var cts = new CancellationTokenSource();

            if (collectionChanged)
            {
                _changeTokenRegistrations = [];
                foreach (var dataSource in _dataSources)
                {
                    _changeTokenRegistrations.Add(ChangeToken.OnChange(
                        dataSource.GetChangeToken,
                        () => HandleChange(collectionChanged: false)));
                }
            }

            _cts = cts;
            _consumerChangeToken = new CancellationChangeToken(cts.Token);
        }

        [MemberNotNull(nameof(_modules))]
        private void CreateModulesUnsynchronized()
        {
            var modules = new List<IModule>();

            foreach (var dataSource in _dataSources)
            {
                modules.AddRange(dataSource.Modules);
            }

            // Only cache _modules after everything succeeds without throwing.
            // We don't want to create a negative cache which would cause 404s when there should be 500s.
            _modules = modules;
        }

        // Use private variable '_modules' to avoid initialization
        private string DebuggerDisplayString => GetDebuggerDisplayStringForModules(_modules);
    }
}

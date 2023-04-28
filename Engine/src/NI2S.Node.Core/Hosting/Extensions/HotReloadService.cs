using Microsoft.Extensions.Primitives;
using System.Threading;
using System;

namespace NI2S.Node.Hosting.Extensions
{
    internal sealed class HotReloadService : IActionDescriptorChangeProvider, IDisposable
    {
        private readonly DefaultModelMetadataProvider? _modelMetadataProvider;
        private readonly DefaultControllerPropertyActivator? _controllerPropertyActivator;
        private readonly RazorHotReload? _razorHotReload;
        private CancellationTokenSource _tokenSource = new();

        public HotReloadService(
            IModelMetadataProvider modelMetadataProvider,
            IControllerPropertyActivator controllerPropertyActivator)
            : this(modelMetadataProvider, controllerPropertyActivator, null)
        {
        }

        public HotReloadService(
            IModelMetadataProvider modelMetadataProvider,
            IControllerPropertyActivator controllerPropertyActivator,
            RazorHotReload? razorHotReload)
        {
            ClearCacheEvent += NotifyClearCache;
            UpdateApplicationEvent += NotifyUpdateApplication;

            if (modelMetadataProvider.GetType() == typeof(DefaultModelMetadataProvider))
            {
                _modelMetadataProvider = (DefaultModelMetadataProvider)modelMetadataProvider;
            }

            if (controllerPropertyActivator is DefaultControllerPropertyActivator defaultControllerPropertyActivator)
            {
                _controllerPropertyActivator = defaultControllerPropertyActivator;
            }

            _razorHotReload = razorHotReload;
        }

        private static event Action<Type[]?>? ClearCacheEvent;
        private static event Action<Type[]?>? UpdateApplicationEvent;

        public static void ClearCache(Type[]? changedTypes)
        {
            ClearCacheEvent?.Invoke(changedTypes);
        }

        public static void UpdateApplication(Type[]? changedTypes)
        {
            UpdateApplicationEvent?.Invoke(changedTypes);
        }

        IChangeToken IActionDescriptorChangeProvider.GetChangeToken() => new CancellationChangeToken(_tokenSource.Token);

        private void NotifyClearCache(Type[]? changedTypes)
        {
            // Clear individual caches
            _modelMetadataProvider?.ClearCache();
            _controllerPropertyActivator?.ClearCache();

            _razorHotReload?.ClearCache(changedTypes);
        }

        private void NotifyUpdateApplication(Type[]? changedTypes)
        {
            // Trigger the ActionDescriptorChangeProvider
            var current = Interlocked.Exchange(ref _tokenSource, new CancellationTokenSource());
            current.Cancel();
        }

        public void Dispose()
        {
            ClearCacheEvent -= NotifyClearCache;
            UpdateApplicationEvent -= NotifyUpdateApplication;
        }
    }
}
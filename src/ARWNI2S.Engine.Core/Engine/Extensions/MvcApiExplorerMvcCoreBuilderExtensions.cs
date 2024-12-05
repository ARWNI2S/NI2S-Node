using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Logging;
using System.Diagnostics.CodeAnalysis;

namespace ARWNI2S.Engine.Extensions
{
    /// <summary>
    /// Extensions for configuring ApiExplorer using an <see cref="INiisCoreBuilder"/>.
    /// </summary>
    public static class NiisApiExplorerNiisCoreBuilderExtensions
    {
        /// <summary>
        /// Configures <see cref="INiisCoreBuilder"/> to use ApiExplorer.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisCoreBuilder AddApiExplorer(this INiisCoreBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            AddApiExplorerServices(builder.Services);
            return builder;
        }

        // Internal for testing.
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        internal static void AddApiExplorerServices(IServiceCollection services)
        {
            services.TryAddSingleton<IApiDescriptionGroupCollectionProvider>(sp => new ApiDescriptionGroupCollectionProvider(
                sp.GetRequiredService<IActionDescriptorCollectionProvider>(),
                sp.GetServices<IApiDescriptionProvider>(),
                sp.GetRequiredService<ILoggerFactory>()));
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApiDescriptionProvider, DefaultApiDescriptionProvider>());
        }
    }
}

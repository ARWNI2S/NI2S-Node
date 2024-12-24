using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ARWNI2S.Clustering.Data
{
    /// <summary>
    /// Extension methods for setting up data protection services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class DataProtectionServiceCollectionExtensions
    {
        /// <summary>
        /// Adds data protection services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        public static IDataProtectionBuilder AddDataProtection(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.TryAddSingleton<IActivator, TypeForwardingActivator>();
            services.AddOptions();
            AddDataProtectionServices(services);

            return new DataProtectionBuilder(services);
        }

        /// <summary>
        /// Adds data protection services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{DataProtectionOptions}"/> to configure the provided <see cref="DataProtectionOptions"/>.</param>
        /// <returns>A reference to this instance after the operation has completed.</returns>
        public static IDataProtectionBuilder AddDataProtection(this IServiceCollection services, Action<DataProtectionOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(setupAction);

            var builder = services.AddDataProtection();
            services.Configure(setupAction);
            return builder;
        }

        private static void AddDataProtectionServices(IServiceCollection services)
        {
            if (OSVersionUtil.IsWindows())
            {
                // Assertion for platform compat analyzer
                Debug.Assert(RuntimeInformation.IsOSPlatform(OSPlatform.Windows));
                services.TryAddSingleton<IRegistryPolicyResolver, RegistryPolicyResolver>();
            }

            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IConfigureOptions<KeyManagementOptions>, KeyManagementOptionsSetup>());
            services.TryAddEnumerable(
                ServiceDescriptor.Singleton<IPostConfigureOptions<KeyManagementOptions>, KeyManagementOptionsPostSetup>());
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<DataProtectionOptions>, DataProtectionOptionsSetup>());

            services.TryAddSingleton<IKeyManager, XmlKeyManager>();
            services.TryAddSingleton<IApplicationDiscriminator, HostingApplicationDiscriminator>();
            services.TryAddEnumerable(ServiceDescriptor.Singleton<IHostedService, DataProtectionHostedService>());

            // Internal services
            services.TryAddSingleton<IDefaultKeyResolver, DefaultKeyResolver>();
            services.TryAddSingleton<IKeyRingProvider, KeyRingProvider>();

            services.TryAddSingleton<IDataProtectionProvider>(s =>
            {
                var dpOptions = s.GetRequiredService<IOptions<DataProtectionOptions>>();
                var keyRingProvider = s.GetRequiredService<IKeyRingProvider>();
                var loggerFactory = s.GetService<ILoggerFactory>() ?? NullLoggerFactory.Instance;

                IDataProtectionProvider dataProtectionProvider = new KeyRingBasedDataProtectionProvider(keyRingProvider, loggerFactory);

                // Link the provider to the supplied discriminator
                if (!string.IsNullOrEmpty(dpOptions.Value.ApplicationDiscriminator))
                {
                    dataProtectionProvider = dataProtectionProvider.CreateProtector(dpOptions.Value.ApplicationDiscriminator);
                }

                return dataProtectionProvider;
            });

            services.TryAddSingleton<ICertificateResolver, CertificateResolver>();
        }
    }
}

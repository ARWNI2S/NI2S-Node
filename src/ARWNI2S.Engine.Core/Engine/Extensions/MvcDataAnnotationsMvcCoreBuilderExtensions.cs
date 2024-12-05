using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ARWNI2S.Engine.Extensions
{
    /// <summary>
    /// Extensions for configuring MVC data annotations using an <see cref="INiisBuilder"/>.
    /// </summary>
    public static class NiisDataAnnotationsNiisCoreBuilderExtensions
    {
        /// <summary>
        /// Registers MVC data annotations.
        /// </summary>
        /// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        /// <returns>The <see cref="INiisBuilder"/>.</returns>
        public static INiisCoreBuilder AddDataAnnotations(this INiisCoreBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            AddDataAnnotationsServices(builder.Services);
            return builder;
        }

        /// <summary>
        /// Adds MVC data annotations localization to the application.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        public static INiisCoreBuilder AddDataAnnotationsLocalization(this INiisCoreBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            return AddDataAnnotationsLocalization(builder, setupAction: null);
        }

        /// <summary>
        /// Registers an action to configure <see cref="NiisDataAnnotationsLocalizationOptions"/> for MVC data
        /// annotations localization.
        /// </summary>
        /// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{NiisDataAnnotationsLocalizationOptions}"/>.</param>
        /// <returns>The <see cref="INiisBuilder"/>.</returns>
        public static INiisCoreBuilder AddDataAnnotationsLocalization(
            this INiisCoreBuilder builder,
            Action<NiisDataAnnotationsLocalizationOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);

            AddDataAnnotationsLocalizationServices(builder.Services, setupAction);
            return builder;
        }

        // Internal for testing.
        internal static void AddDataAnnotationsServices(IServiceCollection services)
        {
            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IConfigureOptions<NiisOptions>, NiisDataAnnotationsNiisOptionsSetup>());
            services.TryAddSingleton<IValidationAttributeAdapterProvider, ValidationAttributeAdapterProvider>();
        }

        // Internal for testing.
        internal static void AddDataAnnotationsLocalizationServices(
            IServiceCollection services,
            Action<NiisDataAnnotationsLocalizationOptions> setupAction)
        {
            DataAnnotationsLocalizationServices.AddDataAnnotationsLocalizationServices(services, setupAction);
        }
    }
}

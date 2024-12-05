using ARWNI2S.Engine.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace ARWNI2S.Engine.Extensions
{
    /// <summary>
    /// Extensions for configuring CORS using an <see cref="INiisCoreBuilder"/>.
    /// </summary>
    public static class NiisCorsNiisCoreBuilderExtensions
    {
        /// <summary>
        /// Configures <see cref="INiisCoreBuilder"/> to use CORS.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        public static INiisCoreBuilder AddCors(this INiisCoreBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            AddCorsServices(builder.Services);
            return builder;
        }

        /// <summary>
        /// Configures <see cref="INiisCoreBuilder"/> to use CORS.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{NiisOptions}"/> to configure the provided <see cref="CorsOptions"/>.</param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        public static INiisCoreBuilder AddCors(
            this INiisCoreBuilder builder,
            Action<CorsOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            AddCorsServices(builder.Services);
            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        /// Configures <see cref="CorsOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <param name="setupAction">The configure action.</param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        public static INiisCoreBuilder ConfigureCors(
            this INiisCoreBuilder builder,
            Action<CorsOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            builder.Services.Configure(setupAction);
            return builder;
        }

        // Internal for testing.
        internal static void AddCorsServices(IServiceCollection services)
        {
            services.AddCors();

            services.TryAddEnumerable(
                ServiceDescriptor.Transient<IApplicationModelProvider, CorsApplicationModelProvider>());
            services.TryAddTransient<CorsAuthorizationFilter, CorsAuthorizationFilter>();
        }
    }
}

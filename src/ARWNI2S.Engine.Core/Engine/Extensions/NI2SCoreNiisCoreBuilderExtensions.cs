using ARWNI2S.ApplicationParts;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace ARWNI2S.Engine.Extensions
{
    /// <summary>
    /// Extensions for configuring NI2S using an <see cref="INiisCoreBuilder"/>.
    /// </summary>
    public static class NI2SCoreNiisCoreBuilderExtensions
    {
        /// <summary>
        /// Registers an action to configure <see cref="NiisOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{NiisOptions}"/>.</param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        public static INiisCoreBuilder AddNiisOptions(
            this INiisCoreBuilder builder,
            Action<NiisOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            builder.Services.Configure(setupAction);
            return builder;
        }

        ///// <summary>
        ///// Configures <see cref="JsonOptions"/> for the specified <paramref name="builder"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        ///// <param name="configure">An <see cref="Action"/> to configure the <see cref="JsonOptions"/>.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //public static INiisCoreBuilder AddJsonOptions(
        //    this INiisCoreBuilder builder,
        //    Action<JsonOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);
        //    ArgumentNullException.ThrowIfNull(configure);

        //    builder.Services.Configure(configure);
        //    return builder;
        //}

        ///// <summary>
        ///// Adds services to support <see cref="FormatterMappings"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //public static INiisCoreBuilder AddFormatterMappings(this INiisCoreBuilder builder)
        //{
        //    AddFormatterMappingsServices(builder.Services);
        //    return builder;
        //}

        ///// <summary>
        ///// Configures <see cref="FormatterMappings"/> for the specified <paramref name="setupAction"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        ///// <param name="setupAction">An <see cref="Action"/> to configure the <see cref="FormatterMappings"/>.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //public static INiisCoreBuilder AddFormatterMappings(
        //    this INiisCoreBuilder builder,
        //    Action<FormatterMappings> setupAction)
        //{
        //    AddFormatterMappingsServices(builder.Services);

        //    if (setupAction != null)
        //    {
        //        builder.Services.Configure<NiisOptions>((options) => setupAction(options.FormatterMappings));
        //    }

        //    return builder;
        //}

        //// Internal for testing.
        //internal static void AddFormatterMappingsServices(IServiceCollection services)
        //{
        //    services.TryAddSingleton<FormatFilter, FormatFilter>();
        //}

        ///// <summary>
        ///// Configures authentication and authorization services for <paramref name="builder"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        ///// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        //public static INiisCoreBuilder AddAuthorization(this INiisCoreBuilder builder)
        //{
        //    AddAuthorizationServices(builder.Services);
        //    return builder;
        //}

        ///// <summary>
        ///// Configures authentication and authorization services for <paramref name="builder"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        ///// <param name="setupAction">An <see cref="Action"/> to configure the <see cref="AuthorizationOptions"/>.</param>
        ///// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        //public static INiisCoreBuilder AddAuthorization(
        //    this INiisCoreBuilder builder,
        //    Action<AuthorizationOptions> setupAction)
        //{
        //    AddAuthorizationServices(builder.Services);

        //    if (setupAction != null)
        //    {
        //        builder.Services.Configure(setupAction);
        //    }

        //    return builder;
        //}

        //// Internal for testing.
        //internal static void AddAuthorizationServices(IServiceCollection services)
        //{
        //    services.AddAuthenticationCore();
        //    services.AddAuthorization();

        //    services.TryAddEnumerable(
        //        ServiceDescriptor.Transient<IApplicationModelProvider, AuthorizationApplicationModelProvider>());
        //}

        ///// <summary>
        ///// Registers discovered controllers as services in the <see cref="IServiceCollection"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        ///// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        //public static INiisCoreBuilder AddControllersAsServices(this INiisCoreBuilder builder)
        //{
        //    var feature = new ControllerFeature();
        //    builder.PartManager.PopulateFeature(feature);

        //    foreach (var controller in feature.Controllers.Select(c => c.AsType()))
        //    {
        //        builder.Services.TryAddTransient(controller, controller);
        //    }

        //    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

        //    return builder;
        //}

        /// <summary>
        /// Adds an <see cref="ApplicationPart"/> to the list of <see cref="ApplicationPartManager.ApplicationParts"/> on the
        /// <see cref="INiisCoreBuilder.PartManager"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <param name="assembly">The <see cref="Assembly"/> of the <see cref="ApplicationPart"/>.</param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        public static INiisCoreBuilder AddApplicationPart(this INiisCoreBuilder builder, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(assembly);

            builder.ConfigureApplicationPartManager(manager =>
            {
                var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                foreach (var applicationPart in partFactory.GetApplicationParts(assembly))
                {
                    manager.ApplicationParts.Add(applicationPart);
                }
            });

            return builder;
        }

        /// <summary>
        /// Configures the <see cref="ApplicationPartManager"/> of the <see cref="INiisCoreBuilder.PartManager"/> using
        /// the given <see cref="Action{ApplicationPartManager}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        /// <param name="setupAction">The <see cref="Action{ApplicationPartManager}"/></param>
        /// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        public static INiisCoreBuilder ConfigureApplicationPartManager(
            this INiisCoreBuilder builder,
            Action<ApplicationPartManager> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            setupAction(builder.PartManager);

            return builder;
        }

        ///// <summary>
        ///// Configures <see cref="ApiBehaviorOptions"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        ///// <param name="setupAction">The configure action.</param>
        ///// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        //public static INiisCoreBuilder ConfigureApiBehaviorOptions(
        //    this INiisCoreBuilder builder,
        //    Action<ApiBehaviorOptions> setupAction)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);
        //    ArgumentNullException.ThrowIfNull(setupAction);

        //    builder.Services.Configure(setupAction);

        //    return builder;
        //}

        ///// <summary>
        ///// Sets the <see cref="CompatibilityVersion"/> for ASP.NET Core NI2S for the application.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisCoreBuilder"/>.</param>
        ///// <param name="version">The <see cref="CompatibilityVersion"/> value to configure.</param>
        ///// <returns>The <see cref="INiisCoreBuilder"/>.</returns>
        //[Obsolete("This API is obsolete and will be removed in a future version. Consider removing usages.",
        //    DiagnosticId = "ASP5001",
        //    UrlFormat = "https://aka.ms/aspnetcore-warnings/{0}")]
        //public static INiisCoreBuilder SetCompatibilityVersion(this INiisCoreBuilder builder, CompatibilityVersion version)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);

        //    builder.Services.Configure<NiisCompatibilityOptions>(o => o.CompatibilityVersion = version);
        //    return builder;
        //}
    }
}

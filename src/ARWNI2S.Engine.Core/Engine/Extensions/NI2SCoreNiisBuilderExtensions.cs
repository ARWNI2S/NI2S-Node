using ARWNI2S.ApplicationParts;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Reflection;

namespace ARWNI2S.Engine.Extensions
{
    /// <summary>
    /// Extensions for configuring NI2S using an <see cref="INiisBuilder"/>.
    /// </summary>
    public static class NI2SCoreNiisBuilderExtensions
    {
        /// <summary>
        /// Registers an action to configure <see cref="NiisOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{NiisOptions}"/>.</param>
        /// <returns>The <see cref="INiisBuilder"/>.</returns>
        public static INiisBuilder AddNiisOptions(
            this INiisBuilder builder,
            Action<NiisOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            builder.Services.Configure(setupAction);
            return builder;
        }

        ///// <summary>
        ///// Configures <see cref="JsonOptions"/> for the specified <paramref name="builder"/>.
        ///// Uses default values from <c>JsonSerializerDefaults.Web</c>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        ///// <param name="configure">An <see cref="Action"/> to configure the <see cref="JsonOptions"/>.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //public static INiisBuilder AddJsonOptions(
        //    this INiisBuilder builder,
        //    Action<JsonOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);
        //    ArgumentNullException.ThrowIfNull(configure);

        //    builder.Services.Configure(configure);
        //    return builder;
        //}

        ///// <summary>
        ///// Configures <see cref="FormatterMappings"/> for the specified <paramref name="builder"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        ///// <param name="setupAction">An <see cref="Action"/> to configure the <see cref="FormatterMappings"/>.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //public static INiisBuilder AddFormatterMappings(
        //    this INiisBuilder builder,
        //    Action<FormatterMappings> setupAction)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);
        //    ArgumentNullException.ThrowIfNull(setupAction);

        //    builder.Services.Configure<NiisOptions>((options) => setupAction(options.FormatterMappings));
        //    return builder;
        //}

        /// <summary>
        /// Adds an <see cref="ApplicationPart"/> to the list of <see cref="ApplicationPartManager.ApplicationParts"/> on the
        /// <see cref="INiisBuilder.PartManager"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        /// <param name="assembly">The <see cref="Assembly"/> of the <see cref="ApplicationPart"/>.</param>
        /// <returns>The <see cref="INiisBuilder"/>.</returns>
        public static INiisBuilder AddApplicationPart(this INiisBuilder builder, Assembly assembly)
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
        /// Configures the <see cref="ApplicationPartManager"/> of the <see cref="INiisBuilder.PartManager"/> using
        /// the given <see cref="Action{ApplicationPartManager}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        /// <param name="setupAction">The <see cref="Action{ApplicationPartManager}"/></param>
        /// <returns>The <see cref="INiisBuilder"/>.</returns>
        public static INiisBuilder ConfigureApplicationPartManager(
            this INiisBuilder builder,
            Action<ApplicationPartManager> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            setupAction(builder.PartManager);

            return builder;
        }

        ///// <summary>
        ///// Registers discovered controllers as services in the <see cref="IServiceCollection"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //public static INiisBuilder AddControllersAsServices(this INiisBuilder builder)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);

        //    var feature = new ControllerFeature();
        //    builder.PartManager.PopulateFeature(feature);

        //    foreach (var controller in feature.Controllers.Select(c => c.AsType()))
        //    {
        //        builder.Services.TryAddTransient(controller, controller);
        //    }

        //    builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

        //    return builder;
        //}

        ///// <summary>
        ///// Sets the <see cref="CompatibilityVersion"/> for ASP.NET Core NI2S for the application.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        ///// <param name="version">The <see cref="CompatibilityVersion"/> value to configure.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //[Obsolete("This API is obsolete and will be removed in a future version. Consider removing usages.",
        //    DiagnosticId = "ASP5001",
        //    UrlFormat = "https://aka.ms/aspnetcore-warnings/{0}")]
        //public static INiisBuilder SetCompatibilityVersion(this INiisBuilder builder, CompatibilityVersion version)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);

        //    builder.Services.Configure<NiisCompatibilityOptions>(o => o.CompatibilityVersion = version);
        //    return builder;
        //}

        ///// <summary>
        ///// Configures <see cref="ApiBehaviorOptions"/>.
        ///// </summary>
        ///// <param name="builder">The <see cref="INiisBuilder"/>.</param>
        ///// <param name="setupAction">The configure action.</param>
        ///// <returns>The <see cref="INiisBuilder"/>.</returns>
        //public static INiisBuilder ConfigureApiBehaviorOptions(
        //    this INiisBuilder builder,
        //    Action<ApiBehaviorOptions> setupAction)
        //{
        //    ArgumentNullException.ThrowIfNull(builder);
        //    ArgumentNullException.ThrowIfNull(setupAction);

        //    builder.Services.Configure(setupAction);

        //    return builder;
        //}
    }
}

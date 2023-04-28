// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Reflection;

namespace NI2S.Node.Hosting.Extensions
{
    /// <summary>
    /// Extensions for configuring NI2S using an <see cref="INI2SBuilder"/>.
    /// </summary>
    public static class NI2SCoreNI2SBuilderExtensions
    {
        /// <summary>
        /// Registers an action to configure <see cref="NI2SOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action{NI2SOptions}"/>.</param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        public static INI2SBuilder AddNI2SOptions(
            this INI2SBuilder builder,
            Action<NI2SOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            builder.Services.Configure(setupAction);
            return builder;
        }

        /// <summary>
        /// Configures <see cref="JsonOptions"/> for the specified <paramref name="builder"/>.
        /// Uses default values from <c>JsonSerializerDefaults.Web</c>.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <param name="configure">An <see cref="Action"/> to configure the <see cref="JsonOptions"/>.</param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        public static INI2SBuilder AddJsonOptions(
            this INI2SBuilder builder)//,
                                      //Action<JsonOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(builder);
            //ArgumentNullException.ThrowIfNull(configure);

            //builder.Services.Configure(configure);
            return builder;
        }

        /// <summary>
        /// Configures <see cref="FormatterMappings"/> for the specified <paramref name="builder"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <param name="setupAction">An <see cref="Action"/> to configure the <see cref="FormatterMappings"/>.</param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        public static INI2SBuilder AddFormatterMappings(
            this INI2SBuilder builder)//,
                                      //Action<FormatterMappings> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            //ArgumentNullException.ThrowIfNull(setupAction);

            //builder.Services.Configure<NI2SOptions>((options) => setupAction(options.FormatterMappings));
            return builder;
        }

        /// <summary>
        /// Adds an <see cref="NI2SPlugin"/> to the list of <see cref="ModuleManager.Plugins"/> on the
        /// <see cref="INI2SBuilder.ModuleManager"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <param name="assembly">The <see cref="Assembly"/> of the <see cref="NI2SPlugin"/>.</param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        public static INI2SBuilder AddNI2SPlugin(this INI2SBuilder builder, Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(assembly);

            builder.ConfigureNI2SModuleManager(manager =>
            {
                //var moduleManager = NI2SPluginFactory.GetNI2SPluginFactory(assembly);
                //foreach (var applicationPart in moduleManager.GetPlugins(assembly))
                //{
                //    manager.Modules.Add(applicationPart);
                //}
            });

            return builder;
        }

        /// <summary>
        /// Configures the <see cref="ModuleManager"/> of the <see cref="INI2SBuilder.ModuleManager"/> using
        /// the given <see cref="Action{NI2SModuleManager}"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <param name="setupAction">The <see cref="Action{NI2SModuleManager}"/></param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        public static INI2SBuilder ConfigureNI2SModuleManager(
            this INI2SBuilder builder,
            Action<IModuleManager> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            ArgumentNullException.ThrowIfNull(setupAction);

            setupAction(builder.ModuleManager);

            return builder;
        }

        /// <summary>
        /// Registers discovered controllers as services in the <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        public static INI2SBuilder AddControllersAsServices(this INI2SBuilder builder)
        {
            ArgumentNullException.ThrowIfNull(builder);

            //var feature = new ControllerFeature();
            //builder.ModuleManager.PopulateFeature(feature);

            //foreach (var controller in feature.Controllers.Select(c => c.AsType()))
            //{
            //    builder.Services.TryAddTransient(controller, controller);
            //}

            //builder.Services.Replace(ServiceDescriptor.Transient<IControllerActivator, ServiceBasedControllerActivator>());

            return builder;
        }

        /// <summary>
        /// Sets the <see cref="CompatibilityVersion"/> for ASP.NET Core NI2S for the engine.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <param name="version">The <see cref="CompatibilityVersion"/> value to configure.</param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        [Obsolete("This API is obsolete and will be removed in a future version. Consider removing usages.",
            DiagnosticId = "ASP5001",
            UrlFormat = "https://aka.ms/aspnetcore-warnings/{0}")]
        public static INI2SBuilder SetCompatibilityVersion(this INI2SBuilder builder, CompatibilityVersion version)
        {
            ArgumentNullException.ThrowIfNull(builder);

            builder.Services.Configure<NI2SCompatibilityOptions>(o => o.CompatibilityVersion = version);
            return builder;
        }

        /// <summary>
        /// Configures <see cref="ApiBehaviorOptions"/>.
        /// </summary>
        /// <param name="builder">The <see cref="INI2SBuilder"/>.</param>
        /// <param name="setupAction">The configure action.</param>
        /// <returns>The <see cref="INI2SBuilder"/>.</returns>
        public static INI2SBuilder ConfigureApiBehaviorOptions(
            this INI2SBuilder builder)//,
                                      //Action<ApiBehaviorOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(builder);
            //ArgumentNullException.ThrowIfNull(setupAction);

            //builder.Services.Configure(setupAction);

            return builder;
        }
    }
}

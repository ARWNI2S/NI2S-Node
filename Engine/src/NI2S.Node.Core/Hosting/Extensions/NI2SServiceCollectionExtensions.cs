// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using Microsoft.Extensions.DependencyInjection;
using NI2S.Node.Engine;
using NI2S.Node.Hosting.Builder;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Reflection;
using System.Reflection.Metadata;

namespace NI2S.Node.Hosting.Extensions
{
    /// <summary>
    /// Extension methods for setting up NI2S services in an <see cref="IServiceCollection" />.
    /// </summary>
    public static class NI2SServiceCollectionExtensions
    {
        /// <summary>
        /// Adds NI2S services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        public static INI2SBuilder AddNI2S(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddControllersWithViews();
            return services.AddRazorPages();
        }

        /// <summary>
        /// Adds NI2S services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{NI2SOptions}"/> to configure the provided <see cref="NI2SOptions"/>.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        public static INI2SBuilder AddNI2S(this IServiceCollection services, Action<NI2SOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(setupAction);

            var builder = services.AddNI2S();
            builder.Services.Configure(setupAction);

            return builder;
        }

        /// <summary>
        /// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        /// register services used for views or pages.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the NI2S services for the commonly used features with controllers for an API. This
        /// combines the effects of <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        /// <see cref="NI2SApiExplorerNI2SCoreBuilderExtensions.AddApiExplorer(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddAuthorization(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCorsNI2SCoreBuilderExtensions.AddCors(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SDataAnnotationsNI2SCoreBuilderExtensions.AddDataAnnotations(INI2SCoreBuilder)"/>,
        /// and <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddFormatterMappings(INI2SCoreBuilder)"/>.
        /// </para>
        /// <para>
        /// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>
        /// on the resulting builder.
        /// </para>
        /// <para>
        /// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>
        /// on the resulting builder.
        /// </para>
        /// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support native AOT.", Url = "https://aka.ms/aspnet/nativeaot")]
        public static INI2SBuilder AddControllers(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = AddControllersCore(services);
            return new NI2SBuilder(builder.Services, builder.ModuleManager);
        }

        /// <summary>
        /// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        /// register services used for views or pages.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">An <see cref="Action{NI2SOptions}"/> to configure the provided <see cref="NI2SOptions"/>.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the NI2S services for the commonly used features with controllers for an API. This
        /// combines the effects of <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        /// <see cref="NI2SApiExplorerNI2SCoreBuilderExtensions.AddApiExplorer(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddAuthorization(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCorsNI2SCoreBuilderExtensions.AddCors(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SDataAnnotationsNI2SCoreBuilderExtensions.AddDataAnnotations(INI2SCoreBuilder)"/>,
        /// and <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddFormatterMappings(INI2SCoreBuilder)"/>.
        /// </para>
        /// <para>
        /// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>
        /// on the resulting builder.
        /// </para>
        /// <para>
        /// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>
        /// on the resulting builder.
        /// </para>
        /// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support native AOT.", Url = "https://aka.ms/aspnet/nativeaot")]
        public static INI2SBuilder AddControllers(this IServiceCollection services, Action<NI2SOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);

            // This method excludes all of the view-related services by default.
            var builder = AddControllersCore(services);
            if (configure != null)
            {
                //builder.AddNI2SOptions(configure);
            }

            return new NI2SBuilder(builder.Services, builder.ModuleManager);
        }

        private static INI2SCoreBuilder AddControllersCore(IServiceCollection services)
        {
            // This method excludes all of the view-related services by default.
            var builder = services
                .AddNI2SCore();
            //.AddApiExplorer()
            //.AddAuthorization()
            //.AddCors()
            //.AddDataAnnotations()
            //.AddFormatterMappings();

            if (MetadataUpdater.IsSupported)
            {
                //services.TryAddEnumerable(
                //    ServiceDescriptor.Singleton<IActionDescriptorChangeProvider, HotReloadService>());
            }

            return builder;
        }

        /// <summary>
        /// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        /// register services used for pages.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the NI2S services for the commonly used features with controllers with views. This
        /// combines the effects of <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        /// <see cref="NI2SApiExplorerNI2SCoreBuilderExtensions.AddApiExplorer(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddAuthorization(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCorsNI2SCoreBuilderExtensions.AddCors(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SDataAnnotationsNI2SCoreBuilderExtensions.AddDataAnnotations(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddFormatterMappings(INI2SCoreBuilder)"/>,
        /// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SViewFeaturesNI2SCoreBuilderExtensions.AddViews(INI2SCoreBuilder)"/>,
        /// and <see cref="NI2SRazorNI2SCoreBuilderExtensions.AddRazorViewEngine(INI2SCoreBuilder)"/>.
        /// </para>
        /// <para>
        /// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>.
        /// </para>
        /// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support native AOT.", Url = "https://aka.ms/aspnet/nativeaot")]
        public static INI2SBuilder AddControllersWithViews(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = AddControllersWithViewsCore(services);
            return new NI2SBuilder(builder.Services, builder.ModuleManager);
        }

        /// <summary>
        /// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        /// register services used for pages.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">An <see cref="Action{NI2SOptions}"/> to configure the provided <see cref="NI2SOptions"/>.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the NI2S services for the commonly used features with controllers with views. This
        /// combines the effects of <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        /// <see cref="NI2SApiExplorerNI2SCoreBuilderExtensions.AddApiExplorer(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddAuthorization(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCorsNI2SCoreBuilderExtensions.AddCors(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SDataAnnotationsNI2SCoreBuilderExtensions.AddDataAnnotations(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddFormatterMappings(INI2SCoreBuilder)"/>,
        /// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SViewFeaturesNI2SCoreBuilderExtensions.AddViews(INI2SCoreBuilder)"/>,
        /// and <see cref="NI2SRazorNI2SCoreBuilderExtensions.AddRazorViewEngine(INI2SCoreBuilder)"/>.
        /// </para>
        /// <para>
        /// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>.
        /// </para>
        /// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support native AOT.", Url = "https://aka.ms/aspnet/nativeaot")]
        public static INI2SBuilder AddControllersWithViews(this IServiceCollection services, Action<NI2SOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);

            // This method excludes all of the view-related services by default.
            var builder = AddControllersWithViewsCore(services);
            if (configure != null)
            {
                //builder.AddNI2SOptions(configure);
            }

            return new NI2SBuilder(builder.Services, builder.ModuleManager);
        }

        private static INI2SCoreBuilder AddControllersWithViewsCore(IServiceCollection services)
        {
            var builder = AddControllersCore(services);
            //.AddViews()
            //.AddRazorViewEngine()
            //.AddCacheTagHelper();

            AddTagHelpersFrameworkParts(builder.ModuleManager);

            return builder;
        }

        /// <summary>
        /// Adds services for pages to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the NI2S services for the commonly used features for pages. This
        /// combines the effects of <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddAuthorization(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SDataAnnotationsNI2SCoreBuilderExtensions.AddDataAnnotations(INI2SCoreBuilder)"/>,
        /// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INI2SCoreBuilder)"/>,
        /// and <see cref="NI2SRazorPagesNI2SCoreBuilderExtensions.AddRazorPages(INI2SCoreBuilder)"/>.
        /// </para>
        /// <para>
        /// To add services for controllers for APIs call <see cref="AddControllers(IServiceCollection)"/>.
        /// </para>
        /// <para>
        /// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>.
        /// </para>
        /// </remarks>
        [RequiresUnreferencedCode("Razor Pages does not currently support native AOT.", Url = "https://aka.ms/aspnet/nativeaot")]
        public static INI2SBuilder AddRazorPages(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = AddRazorPagesCore(services);
            return new NI2SBuilder(builder.Services, builder.ModuleManager);
        }

        /// <summary>
        /// Adds services for pages to the specified <see cref="IServiceCollection"/>.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="configure">An <see cref="Action{NI2SOptions}"/> to configure the provided <see cref="NI2SOptions"/>.</param>
        /// <returns>An <see cref="INI2SBuilder"/> that can be used to further configure the NI2S services.</returns>
        /// <remarks>
        /// <para>
        /// This method configures the NI2S services for the commonly used features for pages. This
        /// combines the effects of <see cref="NI2SCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        /// <see cref="NI2SCoreNI2SCoreBuilderExtensions.AddAuthorization(INI2SCoreBuilder)"/>,
        /// <see cref="NI2SDataAnnotationsNI2SCoreBuilderExtensions.AddDataAnnotations(INI2SCoreBuilder)"/>,
        /// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INI2SCoreBuilder)"/>,
        /// and <see cref="NI2SRazorPagesNI2SCoreBuilderExtensions.AddRazorPages(INI2SCoreBuilder)"/>.
        /// </para>
        /// <para>
        /// To add services for controllers for APIs call <see cref="AddControllers(IServiceCollection)"/>.
        /// </para>
        /// <para>
        /// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>.
        /// </para>
        /// </remarks>
        [RequiresUnreferencedCode("Razor Pages does not currently support native AOT.", Url = "https://aka.ms/aspnet/nativeaot")]
        public static INI2SBuilder AddRazorPages(this IServiceCollection services, Action<RazorPagesOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = AddRazorPagesCore(services);
            if (configure != null)
            {
                //builder.AddRazorPages(configure);
            }

            return new NI2SBuilder(builder.Services, builder.ModuleManager);
        }

        private static INI2SCoreBuilder AddRazorPagesCore(IServiceCollection services)
        {
            // This method includes the minimal things controllers need. It's not really feasible to exclude the services
            // for controllers.
            var builder = services
                .AddNI2SCore();
            //.AddAuthorization()
            //.AddDataAnnotations()
            //.AddRazorPages()
            //.AddCacheTagHelper();

            AddTagHelpersFrameworkParts(builder.ModuleManager);

            if (MetadataUpdater.IsSupported)
            {
                //services.TryAddEnumerable(
                //    ServiceDescriptor.Singleton<IActionDescriptorChangeProvider, HotReloadService>());
            }

            return builder;
        }

        internal static void AddTagHelpersFrameworkParts(IModuleManager moduleManager)
        {
            //var NI2STagHelpersAssembly = typeof(InputTagHelper).Assembly;
            //if (!moduleManager.Plugins.OfType<NI2SPlugin>().Any(p => p.Assembly == NI2STagHelpersAssembly))
            //{
            //    moduleManager.Plugins.Add(new FrameworkNI2SPlugin(NI2STagHelpersAssembly));
            //}

            //var NI2SRazorAssembly = typeof(UrlResolutionTagHelper).Assembly;
            //if (!moduleManager.Plugins.OfType<NI2SPlugin>().Any(p => p.Assembly == NI2SRazorAssembly))
            //{
            //    moduleManager.Plugins.Add(new FrameworkNI2SPlugin(NI2SRazorAssembly));
            //}
        }

        //[DebuggerDisplay("{Name}")]
        //private sealed class FrameworkNI2SPlugin : NI2SPlugin, ICompilationReferencesProvider
        //{
        //    public FrameworkNI2SPlugin(Assembly assembly)
        //        : base(assembly)
        //    {
        //    }

        //    IEnumerable<string> ICompilationReferencesProvider.GetReferencePaths() => Enumerable.Empty<string>();
        //}
    }
}

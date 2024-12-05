using ARWNI2S.ApplicationParts;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Engine.Configuration.Options;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using System.Diagnostics;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Reflection.Metadata;

namespace ARWNI2S.Engine.Extensions
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
        /// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisBuilder AddNI2S(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            services.AddControllersWithViews();
            return services.AddRazorPages();
        }

        /// <summary>
        /// Adds NI2S services to the specified <see cref="IServiceCollection" />.
        /// </summary>
        /// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        /// <param name="setupAction">An <see cref="Action{NiisOptions}"/> to configure the provided <see cref="NiisOptions"/>.</param>
        /// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisBuilder AddNI2S(this IServiceCollection services, Action<NiisOptions> setupAction)
        {
            ArgumentNullException.ThrowIfNull(services);
            ArgumentNullException.ThrowIfNull(setupAction);

            var builder = services.AddNI2S();
            builder.Services.Configure(setupAction);

            return builder;
        }

        ///// <summary>
        ///// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        ///// register services used for views or pages.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        ///// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        ///// <remarks>
        ///// <para>
        ///// This method configures the NI2S services for the commonly used features with controllers for an API. This
        ///// combines the effects of <see cref="NiisCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        ///// <see cref="NiisApiExplorerNiisCoreBuilderExtensions.AddApiExplorer(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddAuthorization(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCorsNiisCoreBuilderExtensions.AddCors(INiisCoreBuilder)"/>,
        ///// <see cref="NiisDataAnnotationsNiisCoreBuilderExtensions.AddDataAnnotations(INiisCoreBuilder)"/>,
        ///// and <see cref="NiisCoreNiisCoreBuilderExtensions.AddFormatterMappings(INiisCoreBuilder)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>
        ///// on the resulting builder.
        ///// </para>
        ///// <para>
        ///// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>
        ///// on the resulting builder.
        ///// </para>
        ///// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisBuilder AddControllers(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = AddControllersCore(services);
            return new NiisBuilder(builder.Services, builder.PartManager);
        }

        ///// <summary>
        ///// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        ///// register services used for views or pages.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        ///// <param name="configure">An <see cref="Action{NiisOptions}"/> to configure the provided <see cref="NiisOptions"/>.</param>
        ///// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        ///// <remarks>
        ///// <para>
        ///// This method configures the NI2S services for the commonly used features with controllers for an API. This
        ///// combines the effects of <see cref="NiisCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        ///// <see cref="NiisApiExplorerNiisCoreBuilderExtensions.AddApiExplorer(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddAuthorization(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCorsNiisCoreBuilderExtensions.AddCors(INiisCoreBuilder)"/>,
        ///// <see cref="NiisDataAnnotationsNiisCoreBuilderExtensions.AddDataAnnotations(INiisCoreBuilder)"/>,
        ///// and <see cref="NiisCoreNiisCoreBuilderExtensions.AddFormatterMappings(INiisCoreBuilder)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>
        ///// on the resulting builder.
        ///// </para>
        ///// <para>
        ///// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>
        ///// on the resulting builder.
        ///// </para>
        ///// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisBuilder AddControllers(this IServiceCollection services, Action<NiisOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);

            // This method excludes all of the view-related services by default.
            var builder = AddControllersCore(services);
            if (configure != null)
            {
                builder.AddNiisOptions(configure);
            }

            return new NiisBuilder(builder.Services, builder.PartManager);
        }

        private static INiisCoreBuilder AddControllersCore(IServiceCollection services)
        {
            // This method excludes all of the view-related services by default.
            var builder = services
                .AddNI2SCore()
                //.AddApiExplorer()
                //.AddAuthorization()
                //.AddCors()
                //.AddDataAnnotations()
                //.AddFormatterMappings()
                ;

            if (MetadataUpdater.IsSupported)
            {
                //services.TryAddEnumerable(
                //    ServiceDescriptor.Singleton<IActionDescriptorChangeProvider, HotReloadService>());
            }

            return builder;
        }

        ///// <summary>
        ///// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        ///// register services used for pages.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        ///// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        ///// <remarks>
        ///// <para>
        ///// This method configures the NI2S services for the commonly used features with controllers with views. This
        ///// combines the effects of <see cref="NiisCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        ///// <see cref="NiisApiExplorerNiisCoreBuilderExtensions.AddApiExplorer(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddAuthorization(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCorsNiisCoreBuilderExtensions.AddCors(INiisCoreBuilder)"/>,
        ///// <see cref="NiisDataAnnotationsNiisCoreBuilderExtensions.AddDataAnnotations(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddFormatterMappings(INiisCoreBuilder)"/>,
        ///// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INiisCoreBuilder)"/>,
        ///// <see cref="NiisViewFeaturesNiisCoreBuilderExtensions.AddViews(INiisCoreBuilder)"/>,
        ///// and <see cref="NiisRazorNiisCoreBuilderExtensions.AddRazorViewEngine(INiisCoreBuilder)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>.
        ///// </para>
        ///// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisBuilder AddControllersWithViews(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = AddControllersWithViewsCore(services);
            return new NiisBuilder(builder.Services, builder.PartManager);
        }

        ///// <summary>
        ///// Adds services for controllers to the specified <see cref="IServiceCollection"/>. This method will not
        ///// register services used for pages.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        ///// <param name="configure">An <see cref="Action{NiisOptions}"/> to configure the provided <see cref="NiisOptions"/>.</param>
        ///// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        ///// <remarks>
        ///// <para>
        ///// This method configures the NI2S services for the commonly used features with controllers with views. This
        ///// combines the effects of <see cref="NiisCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        ///// <see cref="NiisApiExplorerNiisCoreBuilderExtensions.AddApiExplorer(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddAuthorization(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCorsNiisCoreBuilderExtensions.AddCors(INiisCoreBuilder)"/>,
        ///// <see cref="NiisDataAnnotationsNiisCoreBuilderExtensions.AddDataAnnotations(INiisCoreBuilder)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddFormatterMappings(INiisCoreBuilder)"/>,
        ///// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INiisCoreBuilder)"/>,
        ///// <see cref="NiisViewFeaturesNiisCoreBuilderExtensions.AddViews(INiisCoreBuilder)"/>,
        ///// and <see cref="NiisRazorNiisCoreBuilderExtensions.AddRazorViewEngine(INiisCoreBuilder)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for pages call <see cref="AddRazorPages(IServiceCollection)"/>.
        ///// </para>
        ///// </remarks>
        [RequiresUnreferencedCode("NI2S does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisBuilder AddControllersWithViews(this IServiceCollection services, Action<NiisOptions> configure)
        {
            ArgumentNullException.ThrowIfNull(services);

            // This method excludes all of the view-related services by default.
            var builder = AddControllersWithViewsCore(services);
            if (configure != null)
            {
                builder.AddNiisOptions(configure);
            }

            return new NiisBuilder(builder.Services, builder.PartManager);
        }

        private static INiisCoreBuilder AddControllersWithViewsCore(IServiceCollection services)
        {
            var builder = AddControllersCore(services)
                //.AddViews()
                //.AddRazorViewEngine()
                //.AddCacheTagHelper()
                ;

            AddTagHelpersFrameworkParts(builder.PartManager);

            return builder;
        }

        ///// <summary>
        ///// Adds services for pages to the specified <see cref="IServiceCollection"/>.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        ///// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        ///// <remarks>
        ///// <para>
        ///// This method configures the NI2S services for the commonly used features for pages. This
        ///// combines the effects of <see cref="NiisCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddAuthorization(INiisCoreBuilder)"/>,
        ///// <see cref="NiisDataAnnotationsNiisCoreBuilderExtensions.AddDataAnnotations(INiisCoreBuilder)"/>,
        ///// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INiisCoreBuilder)"/>,
        ///// and <see cref="NiisRazorPagesNiisCoreBuilderExtensions.AddRazorPages(INiisCoreBuilder)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for controllers for APIs call <see cref="AddControllers(IServiceCollection)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>.
        ///// </para>
        ///// </remarks>
        [RequiresUnreferencedCode("Razor Pages does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        public static INiisBuilder AddRazorPages(this IServiceCollection services)
        {
            ArgumentNullException.ThrowIfNull(services);

            var builder = AddRazorPagesCore(services);
            return new NiisBuilder(builder.Services, builder.PartManager);
        }

        ///// <summary>
        ///// Adds services for pages to the specified <see cref="IServiceCollection"/>.
        ///// </summary>
        ///// <param name="services">The <see cref="IServiceCollection" /> to add services to.</param>
        ///// <param name="configure">An <see cref="Action{NiisOptions}"/> to configure the provided <see cref="NiisOptions"/>.</param>
        ///// <returns>An <see cref="INiisBuilder"/> that can be used to further configure the NI2S services.</returns>
        ///// <remarks>
        ///// <para>
        ///// This method configures the NI2S services for the commonly used features for pages. This
        ///// combines the effects of <see cref="NiisCoreServiceCollectionExtensions.AddNI2SCore(IServiceCollection)"/>,
        ///// <see cref="NiisCoreNiisCoreBuilderExtensions.AddAuthorization(INiisCoreBuilder)"/>,
        ///// <see cref="NiisDataAnnotationsNiisCoreBuilderExtensions.AddDataAnnotations(INiisCoreBuilder)"/>,
        ///// <see cref="TagHelperServicesExtensions.AddCacheTagHelper(INiisCoreBuilder)"/>,
        ///// and <see cref="NiisRazorPagesNiisCoreBuilderExtensions.AddRazorPages(INiisCoreBuilder)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for controllers for APIs call <see cref="AddControllers(IServiceCollection)"/>.
        ///// </para>
        ///// <para>
        ///// To add services for controllers with views call <see cref="AddControllersWithViews(IServiceCollection)"/>.
        ///// </para>
        ///// </remarks>
        //[RequiresUnreferencedCode("Razor Pages does not currently support trimming or native AOT.", Url = "https://aka.ms/aspnet/trimming")]
        //public static INiisBuilder AddRazorPages(this IServiceCollection services, Action<RazorPagesOptions> configure)
        //{
        //    ArgumentNullException.ThrowIfNull(services);

        //    var builder = AddRazorPagesCore(services);
        //    if (configure != null)
        //    {
        //        builder.AddRazorPages(configure);
        //    }

        //    return new NiisBuilder(builder.Services, builder.PartManager);
        //}

        private static INiisCoreBuilder AddRazorPagesCore(IServiceCollection services)
        {
            // This method includes the minimal things controllers need. It's not really feasible to exclude the services
            // for controllers.
            var builder = services
                .AddNI2SCore()
                //.AddAuthorization()
                //.AddDataAnnotations()
                //.AddRazorPages()
                //.AddCacheTagHelper()
                ;

            AddTagHelpersFrameworkParts(builder.PartManager);

            if (MetadataUpdater.IsSupported)
            {
                //services.TryAddEnumerable(
                //    ServiceDescriptor.Singleton<IActionDescriptorChangeProvider, HotReloadService>());
            }

            return builder;
        }

        internal static void AddTagHelpersFrameworkParts(ApplicationPartManager partManager)
        {
            //var niisTagHelpersAssembly = typeof(InputTagHelper).Assembly;
            //if (!partManager.ApplicationParts.OfType<AssemblyPart>().Any(p => p.Assembly == niisTagHelpersAssembly))
            //{
            //    partManager.ApplicationParts.Add(new FrameworkAssemblyPart(niisTagHelpersAssembly));
            //}

            //var niisRazorAssembly = typeof(UrlResolutionTagHelper).Assembly;
            //if (!partManager.ApplicationParts.OfType<AssemblyPart>().Any(p => p.Assembly == niisRazorAssembly))
            //{
            //    partManager.ApplicationParts.Add(new FrameworkAssemblyPart(niisRazorAssembly));
            //}
        }

        //[DebuggerDisplay("{Name}")]
        //private sealed class FrameworkAssemblyPart : AssemblyPart, ICompilationReferencesProvider
        //{
        //    public FrameworkAssemblyPart(Assembly assembly)
        //        : base(assembly)
        //    {
        //    }

        //    IEnumerable<string> ICompilationReferencesProvider.GetReferencePaths() => Enumerable.Empty<string>();
        //}
    }
}

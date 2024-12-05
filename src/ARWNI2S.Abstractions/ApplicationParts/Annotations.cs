using ARWNI2S.Resources;
using System.Reflection;
using System.Runtime.Loader;

namespace ARWNI2S.ApplicationParts
{
    /// <summary>
    /// Specifies an assembly to be added as an <see cref="ApplicationPart" />.
    /// <para>
    /// In the ordinary case, NI2S will generate <see cref="ApplicationPartAttribute" />
    /// instances on the entry assembly for each dependency that references NI2S.
    /// Each of these assemblies is treated as an <see cref="ApplicationPart" />.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class ApplicationPartAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="ApplicationPartAttribute" />.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        public ApplicationPartAttribute(string assemblyName)
        {
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
        }

        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        public string AssemblyName { get; }
    }

    /// <summary>
    /// Provides a <see cref="ApplicationPartFactory"/> type.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = false)]
    public sealed class ProvideApplicationPartFactoryAttribute : Attribute
    {
        private readonly Type _applicationPartFactoryType;
        private readonly string _applicationPartFactoryTypeName;

        /// <summary>
        /// Creates a new instance of <see cref="ProvideApplicationPartFactoryAttribute"/> with the specified type.
        /// </summary>
        /// <param name="factoryType">The factory type.</param>
        public ProvideApplicationPartFactoryAttribute(Type factoryType)
        {
            _applicationPartFactoryType = factoryType ?? throw new ArgumentNullException(nameof(factoryType));
        }

        /// <summary>
        /// Creates a new instance of <see cref="ProvideApplicationPartFactoryAttribute"/> with the specified type name.
        /// </summary>
        /// <param name="factoryTypeName">The assembly qualified type name.</param>
        public ProvideApplicationPartFactoryAttribute(string factoryTypeName)
        {
            ArgumentException.ThrowIfNullOrEmpty(factoryTypeName);

            _applicationPartFactoryTypeName = factoryTypeName;
        }

        /// <summary>
        /// Gets the factory type.
        /// </summary>
        /// <returns></returns>
        public Type GetFactoryType()
        {
            return _applicationPartFactoryType ??
                Type.GetType(_applicationPartFactoryTypeName!, throwOnError: true)!;
        }
    }

    /// <summary>
    /// Specifies a assembly to load as part of MVRM's assembly discovery mechanism.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class RelatedAssemblyAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="RelatedAssemblyAttribute"/>.
        /// </summary>
        /// <param name="assemblyFileName">The file name, without extension, of the related assembly.</param>
        public RelatedAssemblyAttribute(string assemblyFileName)
        {
            if (string.IsNullOrEmpty(assemblyFileName))
            {
                throw new ArgumentException(LocalizedStrings.ArgumentCannotBeNullOrEmpty, nameof(assemblyFileName));
            }

            AssemblyFileName = assemblyFileName;
        }

        /// <summary>
        /// Gets the assembly file name without extension.
        /// </summary>
        public string AssemblyFileName { get; }

        /// <summary>
        /// Gets <see cref="Assembly"/> instances specified by <see cref="RelatedAssemblyAttribute"/>.
        /// </summary>
        /// <param name="assembly">The assembly containing <see cref="RelatedAssemblyAttribute"/> instances.</param>
        /// <param name="throwOnError">Determines if the method throws if a related assembly could not be located.</param>
        /// <returns>Related <see cref="Assembly"/> instances.</returns>
        public static IReadOnlyList<Assembly> GetRelatedAssemblies(Assembly assembly, bool throwOnError)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            var loadContext = AssemblyLoadContext.GetLoadContext(assembly) ?? AssemblyLoadContext.Default;
            return GetRelatedAssemblies(assembly, throwOnError, File.Exists, new AssemblyLoadContextWrapper(loadContext));
        }

        internal static IReadOnlyList<Assembly> GetRelatedAssemblies(
            Assembly assembly,
            bool throwOnError,
            Func<string, bool> fileExists,
            AssemblyLoadContextWrapper assemblyLoadContext)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            // MVRM will specifically look for related parts in the same physical directory as the assembly.
            // No-op if the assembly does not have a location.
            if (assembly.IsDynamic)
            {
                return Array.Empty<Assembly>();
            }

            var attributes = assembly.GetCustomAttributes<RelatedAssemblyAttribute>().ToArray();
            if (attributes.Length == 0)
            {
                return Array.Empty<Assembly>();
            }

            var assemblyName = assembly.GetName().Name;
            // Assembly.Location may be null for a single-file exe. In this case, attempt to look for related parts in the app's base directory
            var assemblyDirectory = string.IsNullOrEmpty(assembly.Location) ?
                AppContext.BaseDirectory :
                Path.GetDirectoryName(assembly.Location);

            if (string.IsNullOrEmpty(assemblyDirectory))
            {
                return Array.Empty<Assembly>();
            }

            var relatedAssemblies = new List<Assembly>();
            for (var i = 0; i < attributes.Length; i++)
            {
                var attribute = attributes[i];
                if (string.Equals(assemblyName, attribute.AssemblyFileName, StringComparison.OrdinalIgnoreCase))
                {
                    // TODO : RECHOURZES
                    throw new InvalidOperationException(/*Resources.FormatRelatedAssemblyAttribute_AssemblyCannotReferenceSelf(nameof(RelatedAssemblyAttribute), assemblyName)*/);
                }

                Assembly relatedAssembly;
                var relatedAssemblyLocation = Path.Combine(assemblyDirectory, attribute.AssemblyFileName + ".dll");
                if (fileExists(relatedAssemblyLocation))
                {
                    relatedAssembly = assemblyLoadContext.LoadFromAssemblyPath(relatedAssemblyLocation);
                }
                else
                {
                    try
                    {
                        var relatedAssemblyName = new AssemblyName(attribute.AssemblyFileName);
                        relatedAssembly = assemblyLoadContext.LoadFromAssemblyName(relatedAssemblyName);
                    }
                    catch when (!throwOnError)
                    {
                        // Ignore assembly load failures when throwOnError = false.
                        continue;
                    }
                }

                relatedAssemblies.Add(relatedAssembly);
            }

            return relatedAssemblies;
        }

#pragma warning disable CA1852 // Seal internal types
        internal class AssemblyLoadContextWrapper
#pragma warning restore CA1852 // Seal internal types
        {
            private readonly AssemblyLoadContext _loadContext;

            public AssemblyLoadContextWrapper(AssemblyLoadContext loadContext)
            {
                _loadContext = loadContext;
            }

            public virtual Assembly LoadFromAssemblyName(AssemblyName assemblyName)
                => _loadContext.LoadFromAssemblyName(assemblyName);

            public virtual Assembly LoadFromAssemblyPath(string assemblyPath)
                => _loadContext.LoadFromAssemblyPath(assemblyPath);
        }
    }
}

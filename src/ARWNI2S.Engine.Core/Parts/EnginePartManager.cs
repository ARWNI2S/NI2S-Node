using ARWNI2S.Extensibility;
using System.Reflection;

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// Manages the parts and features of an NI2S engine.
    /// </summary>
    public class EnginePartManager : IEnginePartManager
    {
        /// <summary>
        /// Gets the list of <see cref="IEngineModuleProvider"/>s.
        /// </summary>
        public IList<IEngineModuleProvider> ModuleProviders { get; } =
            [];

        /// <summary>
        /// Gets the list of <see cref="EnginePart"/> instances.
        /// <para>
        /// Instances in this collection are stored in precedence order. An <see cref="EnginePart"/> that appears
        /// earlier in the list has a higher precedence.
        /// An <see cref="IEngineModuleProvider"/> may choose to use this an interface as a way to resolve conflicts when
        /// multiple <see cref="EnginePart"/> instances resolve equivalent feature values.
        /// </para>
        /// </summary>
        public IList<EnginePart> EngineParts { get; } = [];

        /// <summary>
        /// Populates the given <paramref name="feature"/> using the list of
        /// <see cref="IEngineModuleProvider{TModule}"/>s configured on the
        /// <see cref="EnginePartManager"/>.
        /// </summary>
        /// <typeparam name="TModule">The type of the feature.</typeparam>
        /// <param name="feature">The feature instance to populate.</param>
        public void PopulateModule<TModule>(TModule feature)
        {
            if (feature == null)
            {
                throw new ArgumentNullException(nameof(feature));
            }

            foreach (var provider in ModuleProviders.OfType<IEngineModuleProvider<TModule>>())
            {
                provider.PopulateModule(EngineParts, feature);
            }
        }

        internal void PopulateDefaultParts(string entryAssemblyName)
        {
            var assemblies = GetEnginePartAssemblies(entryAssemblyName);

            var seenAssemblies = new HashSet<Assembly>();

            foreach (var assembly in assemblies)
            {
                if (!seenAssemblies.Add(assembly))
                {
                    // "assemblies" may contain duplicate values, but we want unique EnginePart instances.
                    // Note that we prefer using a HashSet over Distinct since the latter isn't
                    // guaranteed to preserve the original ordering.
                    continue;
                }

                var partFactory = EnginePartFactory.GetEnginePartFactory(assembly);
                foreach (var enginePart in partFactory.GetEngineParts(assembly))
                {
                    EngineParts.Add(enginePart);
                }
            }
        }

        private static IEnumerable<Assembly> GetEnginePartAssemblies(string entryAssemblyName)
        {
            var entryAssembly = Assembly.Load(new AssemblyName(entryAssemblyName));

            // Use EnginePartAttribute to get the closure of direct or transitive dependencies
            // that reference MVC.
            var assembliesFromAttributes = entryAssembly.GetCustomAttributes<EnginePartAttribute>()
                .Select(name => Assembly.Load(name.AssemblyName))
                .OrderBy(assembly => assembly.FullName, StringComparer.Ordinal)
                .SelectMany(GetAssemblyClosure);

            // The SDK will not include the entry assembly as an engine part. We'll explicitly list it
            // and have it appear before all other assemblies \ EngineParts.
            return GetAssemblyClosure(entryAssembly)
                .Concat(assembliesFromAttributes);
        }

        private static IEnumerable<Assembly> GetAssemblyClosure(Assembly assembly)
        {
            yield return assembly;

            var relatedAssemblies = RelatedAssemblyAttribute.GetRelatedAssemblies(assembly, throwOnError: false)
                .OrderBy(assembly => assembly.FullName, StringComparer.Ordinal);

            foreach (var relatedAssembly in relatedAssemblies)
            {
                yield return relatedAssembly;
            }
        }

        void IEnginePartManager.PopulateDefaultParts(string entryAssemblyName) => PopulateDefaultParts(entryAssemblyName);
    }
}
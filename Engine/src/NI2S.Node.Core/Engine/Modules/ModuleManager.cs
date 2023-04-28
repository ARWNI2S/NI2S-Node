// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace NI2S.Node.Engine
{
    /// <summary>
    /// Manages the parts and modules of an NI2S engine.
    /// </summary>
    public class ModuleManager : IModuleManager
    {
        /// <summary>
        /// Gets the list of <see cref="IEngineModuleProvider"/>s.
        /// </summary>
        public IList<IEngineModuleProvider> Providers { get; } =
            new List<IEngineModuleProvider>();

        /// <summary>
        /// Gets the list of <see cref="NI2SPlugin"/> instances.
        /// <para>
        /// Instances in this collection are stored in precedence order. An <see cref="NI2SPlugin"/> that appears
        /// earlier in the list has a higher precedence.
        /// An <see cref="IEngineModuleProvider"/> may choose to use this an interface as a way to resolve conflicts when
        /// multiple <see cref="NI2SPlugin"/> instances resolve equivalent module values.
        /// </para>
        /// </summary>
        public IList<IEngineModule> Modules { get; } = new List<IEngineModule>();

        /// <summary>
        /// Populates the given <paramref name="module"/> using the list of
        /// <see cref="IEngineModuleProvider{TModule}"/>s configured on the
        /// <see cref="ModuleManager"/>.
        /// </summary>
        /// <typeparam name="TModule">The type of the module.</typeparam>
        /// <param name="module">The module instance to populate.</param>
        public void PopulateModule<TModule>(TModule module)
        {
            if (module == null)
            {
                throw new ArgumentNullException(nameof(module));
            }

            foreach (var provider in Providers.OfType<IEngineModuleProvider<TModule>>())
            {
                provider.PopulateModule(Modules, module);
            }
        }

        internal void PopulateDefaultParts(string entryAssemblyName)
        {
            var assemblies = GetNI2SPluginAssemblies(entryAssemblyName);

            var seenAssemblies = new HashSet<Assembly>();

            foreach (var assembly in assemblies)
            {
                if (!seenAssemblies.Add(assembly))
                {
                    // "assemblies" may contain duplicate values, but we want unique NI2SPlugin instances.
                    // Note that we prefer using a HashSet over Distinct since the latter isn't
                    // guaranteed to preserve the original ordering.
                    continue;
                }

                var moduleFactory = EngineModulesFactory.GetModuleFactory(assembly);
                foreach (var module in moduleFactory.GetModules(assembly))
                {
                    Modules.Add(module);
                }
            }
        }

        private static IEnumerable<Assembly> GetNI2SPluginAssemblies(string entryAssemblyName)
        {
            var entryAssembly = Assembly.Load(new AssemblyName(entryAssemblyName));

            // Use NI2SPluginAttribute to get the closure of direct or transitive dependencies
            // that reference MVC.
            var assembliesFromAttributes = entryAssembly.GetCustomAttributes<NI2SPluginAttribute>()
                .Select(name => Assembly.Load(name.AssemblyName))
                .OrderBy(assembly => assembly.FullName, StringComparer.Ordinal)
                .SelectMany(GetAssemblyClosure);

            // The SDK will not include the entry assembly as an engine part. We'll explicitly list it
            // and have it appear before all other assemblies \ NI2SPlugins.
            return GetAssemblyClosure(entryAssembly).Concat(assembliesFromAttributes);
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
    }
}

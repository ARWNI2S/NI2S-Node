﻿using ARWNI2S.Engine.Features;
using System.Reflection;

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// Manages the parts and features of an MVC application.
    /// </summary>
    public class ApplicationPartManager : IApplicationPartManager
    {
        /// <summary>
        /// Gets the list of <see cref="IEngineFeatureProvider"/>s.
        /// </summary>
        public IList<IEngineFeatureProvider> FeatureProviders { get; } =
            [];

        /// <summary>
        /// Gets the list of <see cref="EnginePart"/> instances.
        /// <para>
        /// Instances in this collection are stored in precedence order. An <see cref="EnginePart"/> that appears
        /// earlier in the list has a higher precedence.
        /// An <see cref="IEngineFeatureProvider"/> may choose to use this an interface as a way to resolve conflicts when
        /// multiple <see cref="EnginePart"/> instances resolve equivalent feature values.
        /// </para>
        /// </summary>
        public IList<EnginePart> EngineParts { get; } = [];

        /// <summary>
        /// Populates the given <paramref name="feature"/> using the list of
        /// <see cref="IEngineFeatureProvider{TFeature}"/>s configured on the
        /// <see cref="ApplicationPartManager"/>.
        /// </summary>
        /// <typeparam name="TFeature">The type of the feature.</typeparam>
        /// <param name="feature">The feature instance to populate.</param>
        public void PopulateFeature<TFeature>(TFeature feature)
        {
            if (feature == null)
            {
                throw new ArgumentNullException(nameof(feature));
            }

            foreach (var provider in FeatureProviders.OfType<IEngineFeatureProvider<TFeature>>())
            {
                provider.PopulateFeature(EngineParts, feature);
            }
        }

        internal void PopulateDefaultParts(string entryAssemblyName)
        {
            var assemblies = GetApplicationPartAssemblies(entryAssemblyName);

            var seenAssemblies = new HashSet<Assembly>();

            foreach (var assembly in assemblies)
            {
                if (!seenAssemblies.Add(assembly))
                {
                    // "assemblies" may contain duplicate values, but we want unique ApplicationPart instances.
                    // Note that we prefer using a HashSet over Distinct since the latter isn't
                    // guaranteed to preserve the original ordering.
                    continue;
                }

                var partFactory = ApplicationPartFactory.GetApplicationPartFactory(assembly);
                foreach (var applicationPart in partFactory.GetApplicationParts(assembly))
                {
                    EngineParts.Add(applicationPart);
                }
            }
        }

        private static IEnumerable<Assembly> GetApplicationPartAssemblies(string entryAssemblyName)
        {
            var entryAssembly = Assembly.Load(new AssemblyName(entryAssemblyName));

            // Use ApplicationPartAttribute to get the closure of direct or transitive dependencies
            // that reference MVC.
            var assembliesFromAttributes = entryAssembly.GetCustomAttributes<ApplicationPartAttribute>()
                .Select(name => Assembly.Load(name.AssemblyName))
                .OrderBy(assembly => assembly.FullName, StringComparer.Ordinal)
                .SelectMany(GetAssemblyClosure);

            // The SDK will not include the entry assembly as an application part. We'll explicitly list it
            // and have it appear before all other assemblies \ ApplicationParts.
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

        void IApplicationPartManager.PopulateDefaultParts(string entryAssemblyName) => PopulateDefaultParts(entryAssemblyName);
    }
}
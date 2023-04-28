// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.


using System;

namespace NI2S.Node.Core.Plugins
{
    /// <summary>
    /// Specifies an assembly to be added as an <see cref="NI2SPlugin" />.
    /// <para>
    /// In the ordinary case, MVC will generate <see cref="NI2SPluginAttribute" />
    /// instances on the entry assembly for each dependency that references MVC.
    /// Each of these assemblies is treated as an <see cref="NI2SPlugin" />.
    /// </para>
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, AllowMultiple = true)]
    public sealed class NI2SPluginAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of <see cref="NI2SPluginAttribute" />.
        /// </summary>
        /// <param name="assemblyName">The assembly name.</param>
        public NI2SPluginAttribute(string assemblyName)
        {
            AssemblyName = assemblyName ?? throw new ArgumentNullException(nameof(assemblyName));
        }

        /// <summary>
        /// Gets the assembly name.
        /// </summary>
        public string AssemblyName { get; }
    }
}
// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Hosting;
using System;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node
{
    /// <summary>
    /// Marker attribute indicating an implementation of <see cref="INodeStartup"/> that will be loaded and executed when building an <see cref="INodeHost"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public sealed class NodeStartupAttribute : Attribute
    {
        /// <summary>
        /// Constructs the <see cref="NodeStartupAttribute"/> with the specified type.
        /// </summary>
        /// <param name="hostingStartupType">A type that implements <see cref="INodeStartup"/>.</param>
        public NodeStartupAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type hostingStartupType)
        {
            if (hostingStartupType == null)
            {
                throw new ArgumentNullException(nameof(hostingStartupType));
            }

            if (!typeof(INodeStartup).IsAssignableFrom(hostingStartupType))
            {
                throw new ArgumentException($@"""{hostingStartupType}"" does not implement {typeof(INodeStartup)}.", nameof(hostingStartupType));
            }

            HostingStartupType = hostingStartupType;
        }

        /// <summary>
        /// The implementation of <see cref="INodeStartup"/> that should be loaded when
        /// starting an engine.
        /// </summary>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public Type HostingStartupType { get; }
    }
}
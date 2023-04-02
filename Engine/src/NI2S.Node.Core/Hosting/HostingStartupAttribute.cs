﻿using System;
using System.Diagnostics.CodeAnalysis;

namespace NI2S.Node.Hosting
{
    /// <summary>
    /// Marker attribute indicating an implementation of <see cref="IHostingStartup"/> that will be loaded and executed when building an <see cref="INodeHost"/>.
    /// </summary>
    [AttributeUsage(AttributeTargets.Assembly, Inherited = false, AllowMultiple = true)]
    public sealed class HostingStartupAttribute : Attribute
    {
        /// <summary>
        /// Constructs the <see cref="HostingStartupAttribute"/> with the specified type.
        /// </summary>
        /// <param name="hostingStartupType">A type that implements <see cref="IHostingStartup"/>.</param>
        public HostingStartupAttribute([DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)] Type hostingStartupType)
        {
            if (hostingStartupType == null)
            {
                throw new ArgumentNullException(nameof(hostingStartupType));
            }

            if (!typeof(IHostingStartup).IsAssignableFrom(hostingStartupType))
            {
                throw new ArgumentException($@"""{hostingStartupType}"" does not implement {typeof(IHostingStartup)}.", nameof(hostingStartupType));
            }

            HostingStartupType = hostingStartupType;
        }

        /// <summary>
        /// The implementation of <see cref="IHostingStartup"/> that should be loaded when
        /// starting an application.
        /// </summary>
        [DynamicallyAccessedMembers(DynamicallyAccessedMemberTypes.PublicParameterlessConstructor)]
        public Type HostingStartupType { get; }
    }
}
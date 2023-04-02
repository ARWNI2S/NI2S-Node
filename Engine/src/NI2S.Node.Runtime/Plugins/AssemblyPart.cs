﻿using System;
using System.Collections.Generic;
using System.Reflection;

namespace NI2S.Node.Plugins
{
    /// <summary>
    /// An <see cref="ApplicationPart"/> backed by an <see cref="System.Reflection.Assembly"/>.
    /// </summary>
    public class AssemblyPart : ApplicationPart, IApplicationPartTypeProvider
    {
        /// <summary>
        /// Initializes a new <see cref="AssemblyPart"/> instance.
        /// </summary>
        /// <param name="assembly">The backing <see cref="System.Reflection.Assembly"/>.</param>
        public AssemblyPart(Assembly assembly)
        {
            Assembly = assembly ?? throw new ArgumentNullException(nameof(assembly));
        }

        /// <summary>
        /// Gets the <see cref="Assembly"/> of the <see cref="ApplicationPart"/>.
        /// </summary>
        public Assembly Assembly { get; }

        /// <summary>
        /// Gets the name of the <see cref="ApplicationPart"/>.
        /// </summary>
        public override string Name => Assembly.GetName().Name!;

        /// <inheritdoc />
        public IEnumerable<TypeInfo> Types => Assembly.DefinedTypes;
    }
}
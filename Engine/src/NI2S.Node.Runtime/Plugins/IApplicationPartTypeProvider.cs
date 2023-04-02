﻿using System.Collections.Generic;
using System.Reflection;

namespace NI2S.Node.Plugins
{
    /// <summary>
    /// Exposes a set of types from an <see cref="ApplicationPart"/>.
    /// </summary>
    public interface IApplicationPartTypeProvider
    {
        /// <summary>
        /// Gets the list of available types in the <see cref="ApplicationPart"/>.
        /// </summary>
        IEnumerable<TypeInfo> Types { get; }
    }
}
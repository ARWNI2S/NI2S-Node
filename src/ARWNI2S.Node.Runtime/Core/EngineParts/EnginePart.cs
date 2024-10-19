﻿namespace ARWNI2S.Runtime.Core.Components
{
    /// <summary>
    /// A part of an NI2S application.
    /// </summary>
    public abstract class EnginePart
    {
        /// <summary>
        /// Gets the <see cref="EnginePart"/> name.
        /// </summary>
        public abstract string Name { get; }
    }
}

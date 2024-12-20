﻿using System.Reflection;

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// Default <see cref="ApplicationPartFactory"/>.
    /// </summary>
    public class DefaultApplicationPartFactory : ApplicationPartFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="DefaultApplicationPartFactory"/>.
        /// </summary>
        public static DefaultApplicationPartFactory Instance { get; } = new DefaultApplicationPartFactory();

        /// <summary>
        /// Gets the sequence of <see cref="EnginePart"/> instances that are created by this instance of <see cref="DefaultApplicationPartFactory"/>.
        /// <para>
        /// Applications may use this method to get the same behavior as this factory produces during MVC's default part discovery.
        /// </para>
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/>.</param>
        /// <returns>The sequence of <see cref="EnginePart"/> instances.</returns>
        public static IEnumerable<EnginePart> GetDefaultApplicationParts(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            yield return new AssemblyPart(assembly);
        }

        /// <inheritdoc />
        public override IEnumerable<EnginePart> GetApplicationParts(Assembly assembly)
        {
            return GetDefaultApplicationParts(assembly);
        }
    }
}
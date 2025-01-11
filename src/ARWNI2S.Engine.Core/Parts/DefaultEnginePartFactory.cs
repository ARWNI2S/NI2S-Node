// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Reflection;

namespace ARWNI2S.Engine.Parts
{
    /// <summary>
    /// Default <see cref="EnginePartFactory"/>.
    /// </summary>
    public class DefaultEnginePartFactory : EnginePartFactory
    {
        /// <summary>
        /// Gets an instance of <see cref="DefaultEnginePartFactory"/>.
        /// </summary>
        public static DefaultEnginePartFactory Instance { get; } = new DefaultEnginePartFactory();

        /// <summary>
        /// Gets the sequence of <see cref="EnginePart"/> instances that are created by this instance of <see cref="DefaultEnginePartFactory"/>.
        /// <para>
        /// Engines may use this method to get the same behavior as this factory produces during MVC's default part discovery.
        /// </para>
        /// </summary>
        /// <param name="assembly">The <see cref="Assembly"/>.</param>
        /// <returns>The sequence of <see cref="EnginePart"/> instances.</returns>
        public static IEnumerable<EnginePart> GetDefaultEngineParts(Assembly assembly)
        {
            ArgumentNullException.ThrowIfNull(assembly);

            yield return new AssemblyPart(assembly);
        }

        /// <inheritdoc />
        public override IEnumerable<EnginePart> GetEngineParts(Assembly assembly)
        {
            return GetDefaultEngineParts(assembly);
        }
    }
}
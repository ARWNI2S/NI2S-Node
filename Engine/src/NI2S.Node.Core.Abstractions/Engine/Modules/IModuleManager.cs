using System.Collections.Generic;
using System.Reflection;

namespace NI2S.Node.Engine
{
    public interface IModuleManager
    {
        /// <summary>
        /// Gets the list of <see cref="IEngineModule"/> instances.
        /// <para>
        /// Instances in this collection are stored in precedence order. An <see cref="IEngineModule"/> that appears
        /// earlier in the list has a higher precedence.
        /// An <see cref="IEngineModuleProvider"/> may choose to use this an interface as a way to resolve conflicts when
        /// multiple <see cref="IEngineModule"/> instances resolve equivalent module values.
        /// </para>
        /// </summary>
        IList<IEngineModule> Modules { get; }

        IList<IEngineModuleProvider> Providers { get; }

        /// <summary>
        /// Find engine modules in an assembly.
        /// </summary>
        /// <param name="assembly">The assembly path where to find modules.</param>
        /// <returns>A <see cref="IList{T}"/> containing assembly modules, if any.</returns>
        //IList<IEngineModule> FindModules(string assembly);

        /// <summary>
        /// Find engine modules in an assembly.
        /// </summary>
        /// <param name="assembly">The assembly where to find modules.</param>
        /// <returns>A <see cref="IList{T}"/> containing assembly modules, if any.</returns>
        //IList<IEngineModule> FindModules(Assembly assembly);
    }
}

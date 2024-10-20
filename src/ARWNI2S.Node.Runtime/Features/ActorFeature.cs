using ARWNI2S.Runtime.EngineParts;
using ARWNI2S.Runtime.Infrastructure.Builder;
using System.Reflection;

namespace ARWNI2S.Runtime.Features
{
    /// <summary>
    /// The list of controllers types in an MVC application. The <see cref="ActorFeature"/> can be populated
    /// using the <see cref="EnginePartManager"/> that is available during startup at <see cref="INI2SCoreBuilder.PartManager"/>
    /// or at a later stage by requiring the <see cref="EnginePartManager"/>
    /// as a dependency in a component.
    /// </summary>
    public class ActorFeature
    {
        /// <summary>
        /// Gets the list of controller types in an MVC application.
        /// </summary>
        public IList<TypeInfo> Controllers { get; } = [];
    }
}

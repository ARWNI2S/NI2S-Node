// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine.Simulation;

namespace ARWNI2S.Engine.Core
{
    /// <summary>
    /// Markup interface to provide a human or ai player representation.
    /// </summary>
    public interface IPlayer
    {
        /// <summary>
        /// The controller that the player is using.
        /// </summary>
        IController Controller { get; }

        /// <summary>
        /// The scene that the player owns.
        /// </summary>
        IScene Scene { get; }
    }
}

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Session
{
    /// <summary>
    /// Provides access to the <see cref="INiisSession"/> for the current process.
    /// </summary>
    public interface ISessionProvider
    {
        /// <summary>
        /// The <see cref="INiisSession"/> for the current process.
        /// </summary>
        INiisSession Session { get; set; }
    }
}

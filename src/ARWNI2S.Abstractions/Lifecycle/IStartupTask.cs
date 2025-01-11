// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Lifecycle
{
    /// <summary>
    /// Defines an action to be taken after silo startup.
    /// </summary>
    public interface IStartupTask
    {
        /// <summary>
        /// Called after the silo has started.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token which is canceled when the method must abort.</param>
        /// <returns>A <see cref="Task"/> representing the work performed.</returns>
        Task Execute(CancellationToken cancellationToken);

        int Order { get; }
    }
}

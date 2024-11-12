namespace ARWNI2S.Node.Core.Lifecycle
{
    /// <summary>
    /// Defines an action to be taken after silo startup.
    /// </summary>
    public interface INodeStartupTask
    {
        /// <summary>
        /// Called after the silo has started.
        /// </summary>
        /// <param name="cancellationToken">The cancellation token which is canceled when the method must abort.</param>
        /// <returns>A <see cref="Task"/> representing the work performed.</returns>
        Task Execute(CancellationToken cancellationToken);
    }
}

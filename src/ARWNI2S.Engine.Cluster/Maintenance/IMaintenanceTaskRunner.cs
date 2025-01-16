namespace ARWNI2S.Cluster.Maintenance
{
    /// <summary>
    /// Schedule task runner interface
    /// </summary>
    public partial interface IMaintenanceTaskRunner
    {
        /// <summary>
        /// Executes the task
        /// </summary>
        /// <param name="scheduleTask">Schedule task</param>
        /// <param name="forceRun">Force run</param>
        /// <param name="throwException">A value indicating whether exception should be thrown if some error happens</param>
        /// <param name="ensureRunOncePerPeriod">A value indicating whether we should ensure this task is run once per run period</param>
        Task ExecuteAsync(MaintenanceTask scheduleTask, bool forceRun = false, bool throwException = false, bool ensureRunOncePerPeriod = true);
    }
}
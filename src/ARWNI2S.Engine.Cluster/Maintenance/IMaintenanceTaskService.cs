namespace ARWNI2S.Cluster.Maintenance
{
    /// <summary>
    /// Task service interface
    /// </summary>
    public partial interface IMaintenanceTaskService
    {
        /// <summary>
        /// Deletes a task
        /// </summary>
        /// <param name="task">Task</param>
        Task DeleteTaskAsync(MaintenanceTask task);

        /// <summary>
        /// Gets a task
        /// </summary>
        /// <param name="taskId">Task identifier</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the schedule task
        /// </returns>
        Task<MaintenanceTask> GetTaskByIdAsync(int taskId);

        /// <summary>
        /// Gets a task by its type
        /// </summary>
        /// <param name="type">Task type</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the schedule task
        /// </returns>
        Task<MaintenanceTask> GetTaskByTypeAsync(string type);

        /// <summary>
        /// Gets all tasks
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the list of schedule task
        /// </returns>
        Task<IList<MaintenanceTask>> GetAllTasksAsync(bool showHidden = false);

        /// <summary>
        /// Inserts a task
        /// </summary>
        /// <param name="task">Task</param>
        Task InsertTaskAsync(MaintenanceTask task);

        /// <summary>
        /// Updates the task
        /// </summary>
        /// <param name="task">Task</param>
        Task UpdateTaskAsync(MaintenanceTask task);
    }
}
namespace ARWNI2S.Cluster.Maintenance
{
    /// <summary>
    /// Represents default values related to task services
    /// </summary>
    public static partial class MaintenanceDefaults
    {
        /// <summary>
        /// Gets a default timeout (in milliseconds) before restarting the application
        /// </summary>
        public static int RestartTimeout => 3000;

        /// <summary>
        /// Gets a path to the database backup files
        /// </summary>
        public static string DbBackupsPath => "db_backups\\";

        /// <summary>
        /// Gets a database backup file extension
        /// </summary>
        public static string DbBackupFileExtension => "bak";

        /// <summary>
        /// Gets a running schedule task path
        /// </summary>
        public static string ScheduleTaskPath => "scheduletask/runtask";
    }
}
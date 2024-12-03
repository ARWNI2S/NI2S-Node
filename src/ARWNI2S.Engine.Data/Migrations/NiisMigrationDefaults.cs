namespace ARWNI2S.Data.Migrations
{
    /// <summary>
    /// Represents default values related to migration process
    /// </summary>
    public static partial class NiisMigrationDefaults
    {
        /// <summary>
        /// Gets the supported datetime formats
        /// </summary>
        public static string[] DateFormats { get; } = ["yyyy-MM-dd HH:mm:ss", "yyyy.MM.dd HH:mm:ss", "yyyy/MM/dd HH:mm:ss", "yyyy-MM-dd HH:mm:ss:fffffff", "yyyy.MM.dd HH:mm:ss:fffffff", "yyyy/MM/dd HH:mm:ss:fffffff"];

        /// <summary>
        /// Gets the format string to create the description of update migration
        /// <remarks>
        /// 0 - dragonCorp full version
        /// 1 - update migration type
        /// </remarks>
        /// </summary>
        public static string UpdateMigrationDescription { get; } = "Third Survey version {0}. Update {1}";

        /// <summary>
        /// Gets the format string to create the description prefix of update migrations
        /// <remarks>
        /// 0 - dragonCorp full version
        /// </remarks>
        /// </summary>
        public static string UpdateMigrationDescriptionPrefix { get; } = "Third Survey version {0}. Update";
    }
}

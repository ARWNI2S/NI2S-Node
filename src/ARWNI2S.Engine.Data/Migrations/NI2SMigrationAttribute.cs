using FluentMigrator;
using System.Globalization;

namespace ARWNI2S.Engine.Data.Migrations
{
    /// <summary>
    /// Attribute for a migration
    /// </summary>
    public partial class NI2SMigrationAttribute : MigrationAttribute
    {
        #region Utils

        protected static long GetVersion(string dateTime)
        {
            return DateTime.ParseExact(dateTime, NI2SMigrationDefaults.DateFormats, CultureInfo.InvariantCulture).Ticks;
        }

        protected static long GetVersion(string dateTime, UpdateMigrationType migrationType)
        {
            return GetVersion(dateTime) + (int)migrationType;
        }

        protected static string GetDescription(string niisVersion, UpdateMigrationType migrationType)
        {
            return string.Format(NI2SMigrationDefaults.UpdateMigrationDescription, niisVersion, migrationType.ToString());
        }

        #endregion

        #region Ctor

        /// <summary>
        /// Initializes a new instance of the ServerMigrationAttribute class
        /// </summary>
        /// <param name="dateTime">The migration date time string to convert on version</param>
        /// <param name="targetMigrationProcess">The target migration process</param>
        public NI2SMigrationAttribute(string dateTime, MigrationProcessType targetMigrationProcess = MigrationProcessType.NoMatter) :
            base(GetVersion(dateTime), null)
        {
            TargetMigrationProcess = targetMigrationProcess;
        }

        /// <summary>
        /// Initializes a new instance of the ServerMigrationAttribute class
        /// </summary>
        /// <param name="dateTime">The migration date time string to convert on version</param>
        /// <param name="description">The migration description</param>
        /// <param name="targetMigrationProcess">The target migration process</param>
        public NI2SMigrationAttribute(string dateTime, string description, MigrationProcessType targetMigrationProcess = MigrationProcessType.NoMatter) :
            base(GetVersion(dateTime), description)
        {
            TargetMigrationProcess = targetMigrationProcess;
        }

        /// <summary>
        /// Initializes a new instance of the ServerMigrationAttribute class
        /// </summary>
        /// <param name="dateTime">The migration date time string to convert on version</param>
        /// <param name="niisVersion">dragonCorp full version</param>
        /// <param name="migrationType">The migration type</param>
        /// <param name="targetMigrationProcess">The target migration process</param>
        public NI2SMigrationAttribute(string dateTime, string niisVersion, UpdateMigrationType migrationType, MigrationProcessType targetMigrationProcess = MigrationProcessType.NoMatter) :
            base(GetVersion(dateTime, migrationType), GetDescription(niisVersion, migrationType))
        {
            TargetMigrationProcess = targetMigrationProcess;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Target migration process
        /// </summary>
        public MigrationProcessType TargetMigrationProcess { get; set; }

        #endregion
    }
}

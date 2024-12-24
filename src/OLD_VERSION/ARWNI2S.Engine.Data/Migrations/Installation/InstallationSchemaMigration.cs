using FluentMigrator;

namespace ARWNI2S.Data.Migrations.Installation
{
    [NI2SMigration("2024/10/11 12:12:12:1212121", "ARWNI2S.Node.Data base schema", MigrationProcessType.Installation)]
    public class InstallationSchemaMigration : AutoReversingMigration
    {
        /// <summary>
        /// Collect the UP migration expressions
        /// <remarks>
        /// We use an explicit table creation order instead of an automatic one
        /// due to problems creating relationships between tables
        /// </remarks>
        /// </summary>
        public override void Up()
        {
            //Clustering
            Create.TableFor<ClusterNode>();
            Create.TableFor<NodeMapping>();

            //Directory
            Create.TableFor<Currency>();
            Create.TableFor<Calendar>();
            Create.TableFor<MeasureTime>();
            Create.TableFor<MeasureDimension>();
            Create.TableFor<MeasureWeight>();
            Create.TableFor<MeasureTemperature>();
            Create.TableFor<CalendarMeasureTimeMapping>();

            //Common
            Create.TableFor<GenericAttribute>();

            //Configuration
            Create.TableFor<Setting>();

            //Localization
            Create.TableFor<Language>();
            Create.TableFor<LocaleStringResource>();
            Create.TableFor<LocalizedProperty>();

            //Users
            Create.TableFor<User>();
            Create.TableFor<UserPassword>();
            Create.TableFor<UserRole>();
            Create.TableFor<UserUserRoleMapping>();

            //Session
            Create.TableFor<SessionRecord>();

            //Logging
            Create.TableFor<ActivityLogType>();
            Create.TableFor<ActivityLog>();
            Create.TableFor<Log>();

            //ScheduleTasks
            Create.TableFor<ScheduleTask>();

            //Security
            Create.TableFor<AclRecord>();
            Create.TableFor<PermissionRecord>();
            Create.TableFor<PermissionRecordUserRoleMapping>();
        }
    }
}

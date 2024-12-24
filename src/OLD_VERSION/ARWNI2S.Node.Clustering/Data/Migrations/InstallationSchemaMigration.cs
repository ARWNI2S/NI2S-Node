using ARWNI2S.Clustering.Data.ScheduleTasks;

namespace ARWNI2S.Clustering.Data.Migrations
{
    [NI2SMigration("2024/10/11 12:12:12:1212121", "ARWNI2S.Clustering.Data schema", MigrationProcessType.Installation)]
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
            Create.TableFor<NI2SNode>();
            Create.TableFor<NodeMapping>();

            //ScheduleTasks
            Create.TableFor<ScheduleTask>();

        }
    }
}

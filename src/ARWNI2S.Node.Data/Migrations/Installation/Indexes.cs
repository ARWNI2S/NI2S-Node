using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Core.Entities.Common;
using ARWNI2S.Node.Core.Entities.Directory;
using ARWNI2S.Node.Core.Entities.Localization;
using ARWNI2S.Node.Core.Entities.Logging;
using ARWNI2S.Node.Core.Entities.Security;
using ARWNI2S.Node.Core.Entities.Users;
using FluentMigrator;

namespace ARWNI2S.Node.Data.Migrations.Installation
{
    [NI2SMigration("2024/10/11 13:13:13:1313131", "ARWNI2S.Node.Data base indexes", MigrationProcessType.Installation)]
    public class Indexes : AutoReversingMigration
    {
        #region Methods

        public override void Up()
        {
            Create.Index("IX_NI2SNode_NodeId").OnTable(nameof(ClusterNode))
                .OnColumn(nameof(ClusterNode.NodeId)).Unique()
                .WithOptions().NonClustered();

            Create.Index("IX_NodeMapping_EntityId_EntityName").OnTable(nameof(NodeMapping))
                .OnColumn(nameof(NodeMapping.EntityId)).Ascending()
                .OnColumn(nameof(NodeMapping.EntityName)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_Log_CreatedOnUtc").OnTable(nameof(Log))
                .OnColumn(nameof(Log.CreatedOnUtc)).Descending()
                .WithOptions().NonClustered();

            Create.Index("IX_LocaleStringResource").OnTable(nameof(LocaleStringResource))
                .OnColumn(nameof(LocaleStringResource.ResourceName)).Ascending()
                .OnColumn(nameof(LocaleStringResource.LanguageId)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_Language_DisplayOrder").OnTable(nameof(Language))
                .OnColumn(nameof(Language.DisplayOrder)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_GenericAttribute_EntityId_and_KeyGroup").OnTable(nameof(GenericAttribute))
                .OnColumn(nameof(GenericAttribute.EntityId)).Ascending()
                .OnColumn(nameof(GenericAttribute.KeyGroup)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_User_Username").OnTable(nameof(User))
                .OnColumn(nameof(User.Username)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_User_SystemName").OnTable(nameof(User))
                .OnColumn(nameof(User.SystemName)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_User_Email").OnTable(nameof(User))
                .OnColumn(nameof(User.Email)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_User_UserGuid").OnTable(nameof(User))
                .OnColumn(nameof(User.UserGuid)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_User_CreatedOnUtc").OnTable(nameof(User))
                .OnColumn(nameof(User.CreatedOnUtc)).Descending()
                .WithOptions().NonClustered();

            Create.Index("IX_Currency_DisplayOrder").OnTable(nameof(Currency))
                .OnColumn(nameof(Currency.DisplayOrder)).Ascending()
                .WithOptions().NonClustered();

            Create.Index("IX_ActivityLog_CreatedOnUtc").OnTable(nameof(ActivityLog))
                .OnColumn(nameof(ActivityLog.CreatedOnUtc)).Descending()
                .WithOptions().NonClustered();

            Create.Index("IX_AclRecord_EntityId_EntityName").OnTable(nameof(AclRecord))
                .OnColumn(nameof(AclRecord.EntityId)).Ascending()
                .OnColumn(nameof(AclRecord.EntityName)).Ascending()
                .WithOptions().NonClustered();
        }

        #endregion
    }
}

using FluentMigrator;

namespace ARWNI2S.Node.Data.Migrations.Installation
{
    [NI2SMigration("2023/07/28 13:13:13:1313131", "ARWNI2S.Node.Data base indexes", MigrationProcessType.Installation)]
    public class Indexes : AutoReversingMigration
    {
        #region Methods

        public override void Up()
        {
            //Create.Index("IX_VirtualFileRecord_Slug")
            //    .OnTable(nameof(VirtualFileRecord))
            //    .OnColumn(nameof(VirtualFileRecord.Slug))
            //.Ascending()
            //.WithOptions()
            //    .NonClustered();

            //Create.Index("IX_VirtualFileRecord_Custom_1").OnTable(nameof(VirtualFileRecord))
            //    .OnColumn(nameof(VirtualFileRecord.EntityId)).Ascending()
            //    .OnColumn(nameof(VirtualFileRecord.EntityName)).Ascending()
            //    .OnColumn(nameof(VirtualFileRecord.LanguageId)).Ascending()
            //    .OnColumn(nameof(VirtualFileRecord.IsActive)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_ServerMapping_EntityId_EntityName").OnTable(nameof(ServerMapping))
            //    .OnColumn(nameof(ServerMapping.EntityId)).Ascending()
            //    .OnColumn(nameof(ServerMapping.EntityName)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_QueuedEmail_SentOnUtc_DontSendBeforeDateUtc_Extended").OnTable(nameof(QueuedEmail))
            //    .OnColumn(nameof(QueuedEmail.SentOnUtc)).Ascending()
            //    .OnColumn(nameof(QueuedEmail.DontSendBeforeDateUtc)).Ascending()
            //    .WithOptions().NonClustered()
            //    .Include(nameof(QueuedEmail.SentTries));

            //Create.Index("IX_QueuedEmail_CreatedOnUtc").OnTable(nameof(QueuedEmail))
            //    .OnColumn(nameof(QueuedEmail.CreatedOnUtc)).Descending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_NewsletterSubscription_Email_ServerId").OnTable(nameof(NewsLetterSubscription))
            //    .OnColumn(nameof(NewsLetterSubscription.Email)).Ascending()
            //    .OnColumn(nameof(NewsLetterSubscription.ServerId)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_Log_CreatedOnUtc").OnTable(nameof(Log))
            //    .OnColumn(nameof(Log.CreatedOnUtc)).Descending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_LocaleStringResource").OnTable(nameof(LocaleStringResource))
            //    .OnColumn(nameof(LocaleStringResource.ResourceName)).Ascending()
            //    .OnColumn(nameof(LocaleStringResource.LanguageId)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_Language_DisplayOrder").OnTable(nameof(Language))
            //    .OnColumn(nameof(Language.DisplayOrder)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_GenericAttribute_EntityId_and_KeyGroup").OnTable(nameof(GenericAttribute))
            //    .OnColumn(nameof(GenericAttribute.EntityId)).Ascending()
            //    .OnColumn(nameof(GenericAttribute.KeyGroup)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_User_Username").OnTable(nameof(User))
            //    .OnColumn(nameof(User.Username)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_User_SystemName").OnTable(nameof(User))
            //    .OnColumn(nameof(User.SystemName)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_User_Email").OnTable(nameof(User))
            //    .OnColumn(nameof(User.Email)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_User_UserGuid").OnTable(nameof(User))
            //    .OnColumn(nameof(User.UserGuid)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_User_CreatedOnUtc").OnTable(nameof(User))
            //    .OnColumn(nameof(User.CreatedOnUtc)).Descending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_Currency_DisplayOrder").OnTable(nameof(Currency))
            //    .OnColumn(nameof(Currency.DisplayOrder)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_Country_DisplayOrder").OnTable(nameof(Country))
            //    .OnColumn(nameof(Country.DisplayOrder)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_ActivityLog_CreatedOnUtc").OnTable(nameof(ActivityLog))
            //    .OnColumn(nameof(ActivityLog.CreatedOnUtc)).Descending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_AclRecord_EntityId_EntityName").OnTable(nameof(AclRecord))
            //    .OnColumn(nameof(AclRecord.EntityId)).Ascending()
            //    .OnColumn(nameof(AclRecord.EntityName)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_Token_DisplayOrder").OnTable(nameof(Token))
            //    .OnColumn(nameof(Token.DisplayOrder)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_QuestGoal_QuestId_GoalId").OnTable(nameof(QuestGoal))
            //    .OnColumn(nameof(QuestGoal.QuestId)).Ascending()
            //    .OnColumn(nameof(QuestGoal.GoalId)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.Index("IX_QuestReward_QuestId_RewardId").OnTable(nameof(QuestReward))
            //    .OnColumn(nameof(QuestReward.QuestId)).Ascending()
            //    .OnColumn(nameof(QuestReward.RewardId)).Ascending()
            //    .WithOptions().NonClustered();

            //Create.UniqueConstraint("UK_CryptoAddress_Address_BlockchainId").OnTable(nameof(CryptoAddress))
            //    .Columns(nameof(CryptoAddress.Address), nameof(CryptoAddress.BlockchainId))
            //    .NonClustered();
        }

        #endregion
    }
}

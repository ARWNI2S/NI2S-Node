namespace ARWNI2S.Node.Data.Migrations.Installation
{
    [MetalinkMigration("2023/07/28 12:12:12:1212121", "TheCorporateWars.Server.Data base schema", MigrationProcessType.Installation)]
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
            //Nodes
            Create.TableFor<Node>();
            Create.TableFor<NodeMapping>();

            //Directory
            Create.TableFor<Country>();
            Create.TableFor<Currency>();
            Create.TableFor<MeasureDimension>();
            Create.TableFor<MeasureWeight>();
            Create.TableFor<StateProvince>();

            //Common
            Create.TableFor<Address>();
            Create.TableFor<AddressAttribute>();
            Create.TableFor<AddressAttributeValue>();
            Create.TableFor<GenericAttribute>();
            Create.TableFor<SearchTerm>();

            //Affiliates
            Create.TableFor<Affiliate>();

            //Configuration
            Create.TableFor<Setting>();

            //Localization
            Create.TableFor<Language>();
            Create.TableFor<LocaleStringResource>();
            Create.TableFor<LocalizedProperty>();

            //Blockchains
            Create.TableFor<Token>();
            Create.TableFor<Blockchain>();
            Create.TableFor<BlockchainTokenMapping>();
            Create.TableFor<NonFungibleToken>();
            Create.TableFor<SmartContract>();

            //Users
            Create.TableFor<User>();
            Create.TableFor<UserAttribute>();
            Create.TableFor<UserAttributeValue>();
            Create.TableFor<UserPassword>();
            Create.TableFor<UserAddressMapping>();
            Create.TableFor<UserRole>();
            Create.TableFor<UserUserRoleMapping>();
            Create.TableFor<ExternalAuthenticationRecord>();

            //Partners
            Create.TableFor<Partner>();
            Create.TableFor<PartnerAttribute>();
            Create.TableFor<PartnerAttributeValue>();
            Create.TableFor<PartnerNote>();

            //Wallets
            //Create.TableFor<Wallet>();
            Create.TableFor<CryptoAddress>();
            Create.TableFor<WalletAuthenticationRecord>();

            //Gameplay
            Create.TableFor<EnrollmentRequirement>();

            //Gameplay/Governance
            Create.TableFor<Rank>();

            //Gameplay/Players
            Create.TableFor<ExperienceLevel>();
            Create.TableFor<Player>();
            Create.TableFor<PlayerAttribute>();
            Create.TableFor<PlayerAttributeValue>();
            Create.TableFor<PlayerNote>();
            Create.TableFor<ExperiencePointsHistory>();
            Create.TableFor<InventoryItem>();
            Create.TableFor<PlayerInventoryItem>();
            Create.TableFor<SystemMessage>();

            //Gameplay/Rewards
            Create.TableFor<GoalType>();
            Create.TableFor<Goal>();
            Create.TableFor<GoalAttribute>();
            Create.TableFor<GoalAttributeValue>();
            Create.TableFor<Reward>();

            //Gameplay/Achievements
            Create.TableFor<Achievement>();
            Create.TableFor<AchievementUnlock>();

            //Gameplay/Quests
            Create.TableFor<Quest>();
            Create.TableFor<QuestEnrollment>();
            Create.TableFor<QuestEnrollmentRequirementMapping>();
            Create.TableFor<QuestGoal>();
            Create.TableFor<QuestReward>();

            //Gameplay/Tournaments
            Create.TableFor<Tournament>();
            Create.TableFor<TournamentEnrollment>();
            Create.TableFor<TournamentEnrollmentRequirementMapping>();
            Create.TableFor<Match>();

            //Gdpr
            Create.TableFor<GdprConsent>();
            Create.TableFor<GdprLog>();

            //Blogs
            Create.TableFor<BlogPost>();
            Create.TableFor<BlogComment>();

            //Logging
            Create.TableFor<ActivityLogType>();
            Create.TableFor<ActivityLog>();
            Create.TableFor<Log>();

            //Media
            Create.TableFor<Download>();
            Create.TableFor<Picture>();
            Create.TableFor<PictureBinary>();
            Create.TableFor<QuestPicture>();
            Create.TableFor<TournamentPicture>();
            Create.TableFor<Video>();
            Create.TableFor<QuestVideo>();
            Create.TableFor<TournamentVideo>();

            //Games
            Create.TableFor<Platform>();
            Create.TableFor<Genre>();
            Create.TableFor<Publisher>();
            Create.TableFor<Title>();
            Create.TableFor<TitleGenre>();
            Create.TableFor<TitlePlatform>();
            Create.TableFor<TitlePicture>();
            Create.TableFor<TitleVideo>();

            //Messages
            Create.TableFor<Campaign>();
            Create.TableFor<EmailAccount>();
            Create.TableFor<MessageTemplate>();
            Create.TableFor<NewsLetterSubscription>();
            Create.TableFor<QueuedEmail>();

            //News
            Create.TableFor<NewsItem>();
            Create.TableFor<NewsComment>();

            //Polls
            Create.TableFor<Poll>();
            Create.TableFor<PollAnswer>();
            Create.TableFor<PollVotingRecord>();

            //ScheduleTasks
            Create.TableFor<ScheduleTask>();

            //Security
            Create.TableFor<AclRecord>();
            Create.TableFor<PermissionRecord>();
            Create.TableFor<PermissionRecordUserRoleMapping>();

            //Seo
            Create.TableFor<UrlRecord>();

            //Tax
            Create.TableFor<TaxCategory>();

            //Topics
            Create.TableFor<TopicTemplate>();
            Create.TableFor<Topic>();

            //Metaverses
            Create.TableFor<MetaverseEntityMapping>();
        }
    }
}

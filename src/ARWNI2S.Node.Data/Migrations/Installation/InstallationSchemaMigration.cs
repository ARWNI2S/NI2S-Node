using FluentMigrator;

namespace ARWNI2S.Node.Data.Migrations.Installation
{
    [ServerMigration("2024/10/11 12:12:12:1212121", "ARWNI2S.Node.Data base schema", MigrationProcessType.Installation)]
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
            ////Servers
            //Create.TableFor<BladeServer>();
            //Create.TableFor<ServerMapping>();

            ////Directory
            //Create.TableFor<Country>();
            //Create.TableFor<Currency>();
            //Create.TableFor<MeasureDimension>();
            //Create.TableFor<MeasureWeight>();
            //Create.TableFor<StateProvince>();

            ////Common
            //Create.TableFor<Address>();
            //Create.TableFor<AddressAttribute>();
            //Create.TableFor<AddressAttributeValue>();
            //Create.TableFor<GenericAttribute>();
            //Create.TableFor<SearchTerm>();

            ////Configuration
            //Create.TableFor<Setting>();

            ////Localization
            //Create.TableFor<Language>();
            //Create.TableFor<LocaleStringResource>();
            //Create.TableFor<LocalizedProperty>();

            ////Users
            //Create.TableFor<User>();
            //Create.TableFor<UserAttribute>();
            //Create.TableFor<UserAttributeValue>();
            //Create.TableFor<UserPassword>();
            //Create.TableFor<UserAddressMapping>();
            //Create.TableFor<UserRole>();
            //Create.TableFor<UserUserRoleMapping>();
            //Create.TableFor<ExternalAuthenticationRecord>();

            ////Assets
            //Create.TableFor<UnrealAsset>();

            ////Gameplay

            ////Gameplay/Configuration
            //Create.TableFor<GameplaySetting>();

            ////Gameplay/Localization
            //Create.TableFor<GameLanguage>();
            //Create.TableFor<LocaleGameStringResource>();
            //Create.TableFor<LocalizedGameplayProperty>();

            ////Gdpr
            //Create.TableFor<GdprConsent>();
            //Create.TableFor<GdprLog>();

            ////Logging
            //Create.TableFor<ActivityLogType>();
            //Create.TableFor<ActivityLog>();
            //Create.TableFor<Log>();

            ////Media
            //Create.TableFor<Download>();
            //Create.TableFor<Picture>();
            //Create.TableFor<PictureBinary>();
            //Create.TableFor<Video>();

            ////Messages
            //Create.TableFor<Campaign>();
            //Create.TableFor<EmailAccount>();
            //Create.TableFor<MessageTemplate>();
            //Create.TableFor<NewsLetterSubscription>();
            //Create.TableFor<QueuedEmail>();
            //Create.TableFor<SystemMessage>();

            ////ScheduleTasks
            //Create.TableFor<ScheduleTask>();

            ////Security
            //Create.TableFor<AclRecord>();
            //Create.TableFor<PermissionRecord>();
            //Create.TableFor<PermissionRecordUserRoleMapping>();
        }
    }
}

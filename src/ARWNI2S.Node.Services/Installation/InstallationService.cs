using ARWNI2S.Infrastructure;
using ARWNI2S.Node.Core;
using ARWNI2S.Node.Core.Configuration;
using ARWNI2S.Node.Core.Entities;
using ARWNI2S.Node.Core.Entities.Clustering;
using ARWNI2S.Node.Core.Entities.Common;
using ARWNI2S.Node.Core.Entities.Directory;
using ARWNI2S.Node.Core.Entities.Localization;
using ARWNI2S.Node.Core.Entities.Logging;
using ARWNI2S.Node.Core.Entities.ScheduleTasks;
using ARWNI2S.Node.Core.Entities.Security;
using ARWNI2S.Node.Core.Entities.Users;
using ARWNI2S.Node.Core.Infrastructure;
using ARWNI2S.Node.Core.Network;
using ARWNI2S.Node.Core.Services.Helpers;
using ARWNI2S.Node.Data;
using ARWNI2S.Node.Data.Extensions;
using ARWNI2S.Node.Services.Common;
using ARWNI2S.Node.Services.Configuration;
using ARWNI2S.Node.Services.Localization;
using ARWNI2S.Node.Services.Security;
using ARWNI2S.Node.Services.Users;
using System.Globalization;
using System.Net;

namespace ARWNI2S.Node.Services.Installation
{
    /// <summary>
    /// Installation service
    /// </summary>
    public partial class InstallationService : IInstallationService
	{
		#region Fields

		private readonly NI2SSettings _nI2SSettings;
		private readonly INI2SDataProvider _dataProvider;
		private readonly IEngineFileProvider _fileProvider;
		private readonly IRepository<ActivityLogType> _activityLogTypeRepository;
		private readonly IRepository<Currency> _currencyRepository;
		private readonly IRepository<User> _userRepository;
		private readonly IRepository<UserRole> _userRoleRepository;
		private readonly IRepository<Language> _languageRepository;
		private readonly IRepository<MeasureDimension> _measureDimensionRepository;
		private readonly IRepository<MeasureWeight> _measureWeightRepository;
		private readonly IRepository<NI2SNode> _nodeRepository;
		private readonly INI2SNetHelper _nodeHelper;

		#endregion

		#region Ctor

		public InstallationService(NI2SSettings nI2SSettings,
			INI2SDataProvider dataProvider,
			IEngineFileProvider fileProvider,
			IRepository<ActivityLogType> activityLogTypeRepository,
			IRepository<Currency> currencyRepository,
			IRepository<User> userRepository,
			IRepository<UserRole> userRoleRepository,
			IRepository<Language> languageRepository,
			IRepository<MeasureDimension> measureDimensionRepository,
			IRepository<MeasureWeight> measureWeightRepository,
			IRepository<NI2SNode> nodeRepository,
			INI2SNetHelper nodeHelper)
		{
			_nI2SSettings = nI2SSettings;
			_dataProvider = dataProvider;
			_fileProvider = fileProvider;
			_activityLogTypeRepository = activityLogTypeRepository;
			_currencyRepository = currencyRepository;
			_userRepository = userRepository;
			_userRoleRepository = userRoleRepository;
			_languageRepository = languageRepository;
			_measureDimensionRepository = measureDimensionRepository;
			_measureWeightRepository = measureWeightRepository;
			_nodeRepository = nodeRepository;
			_nodeHelper = nodeHelper;
		}

		#endregion

		#region Utilities

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task<T> InsertInstallationDataAsync<T>(T entity) where T : DataEntity
		{
			return await _dataProvider.InsertEntityAsync(entity);
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InsertInstallationDataAsync<T>(params T[] entities) where T : DataEntity
		{
			await _dataProvider.BulkInsertEntitiesAsync(entities);
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InsertInstallationDataAsync<T>(IList<T> entities) where T : DataEntity
		{
			if (!entities.Any())
				return;

			await InsertInstallationDataAsync(entities.ToArray());
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task UpdateInstallationDataAsync<T>(T entity) where T : DataEntity
		{
			await _dataProvider.UpdateEntityAsync(entity);
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task UpdateInstallationDataAsync<T>(IList<T> entities) where T : DataEntity
		{
			if (!entities.Any())
				return;

			foreach (var entity in entities)
				await _dataProvider.UpdateEntityAsync(entity);
		}

		#endregion

		#region Data Requeriments

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallNodeAsync()
		{
			var nodeConfig = _nI2SSettings.Get<NodeConfig>();
			var nodes = new List<NI2SNode>
			{
				new() {
					Name = nodeConfig.NodeName,
					CurrentState = NodeState.Joining,
					Hosts = $"{Dns.GetHostName()}:{nodeConfig.Port}",
					IpAddress = _nodeHelper.GetCurrentIpAddress(),
					NodeId = nodeConfig.NodeId,
					PublicPort = nodeConfig.Port,
					RelayPort = nodeConfig.RelayPort,
					SslEnabled = false,
				}
			};

			await InsertInstallationDataAsync(nodes);
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallMeasuresAsync(RegionInfo regionInfo)
		{
			var isMetric = regionInfo?.IsMetric ?? false;

			var measureDimensions = new List<MeasureDimension>
			{
				new() {
					Name = "inch(es)",
					SystemKeyword = SystemKeywords.Measures.Inches,
					Ratio = isMetric ? 39.3701M : 1M,
					DisplayOrder = isMetric ? 1 : 0
				},
				new() {
					Name = "feet",
					SystemKeyword = SystemKeywords.Measures.Feet,
					Ratio = isMetric ? 3.28084M : 0.08333333M,
					DisplayOrder = isMetric ? 1 : 0
				},
				new() {
					Name = "meter(s)",
					SystemKeyword = SystemKeywords.Measures.Meters,
					Ratio = isMetric ? 1M : 0.0254M,
					DisplayOrder = isMetric ? 0 : 1
				},
				new() {
					Name = "millimetre(s)",
					SystemKeyword = SystemKeywords.Measures.Millimetres,
					Ratio = isMetric ? 1000M : 25.4M,
					DisplayOrder = isMetric ? 0 : 1
				}
			};

			await InsertInstallationDataAsync(measureDimensions);

			var measureWeights = new List<MeasureWeight>
			{
				new() {
					Name = "ounce(s)",
					SystemKeyword = SystemKeywords.Measures.Ounces,
					Ratio = isMetric ? 35.274M : 16M,
					DisplayOrder = isMetric ? 1 : 0
				},
				new() {
					Name = "lb(s)",
					SystemKeyword = SystemKeywords.Measures.Pounds,
					Ratio = isMetric ? 2.20462M : 1M,
					DisplayOrder = isMetric ? 1 : 0
				},
				new() {
					Name = "kg(s)",
					SystemKeyword = SystemKeywords.Measures.Kilograms,
					Ratio = isMetric ? 1M : 0.45359237M,
					DisplayOrder = isMetric ? 0 : 1
				},
				new() {
					Name = "gram(s)",
					SystemKeyword = SystemKeywords.Measures.Grams,
					Ratio = isMetric ? 1000M : 453.59237M,
					DisplayOrder = isMetric ? 0 : 1
				}
			};

			await InsertInstallationDataAsync(measureWeights);
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallLanguagesAsync((string languagePackDownloadLink, int languagePackProgress) languagePackInfo, CultureInfo cultureInfo, RegionInfo regionInfo)
		{
			var localizationService = EngineContext.Current.Resolve<ILocalizationService>();

			var defaultCulture = new CultureInfo(CommonServicesDefaults.DefaultLanguageCulture);
			var defaultLanguage = new Language
			{
				Name = defaultCulture.NativeName,
				LanguageCulture = defaultCulture.Name,
				UniqueSeoCode = defaultCulture.TwoLetterISOLanguageName,
				FlagImageFileName = $"{defaultCulture.Name.ToLowerInvariant()[^2..]}.png",
				Rtl = defaultCulture.TextInfo.IsRightToLeft,
				Published = true,
				DisplayOrder = 1
			};
			await InsertInstallationDataAsync(defaultLanguage);

			//Install locale resources for default culture
			var directoryPath = _fileProvider.MapPath(InstallationDefaults.LocalizationResourcesPath);
			var pattern = $"*.{InstallationDefaults.LocalizationResourcesFileExtension}";
			foreach (var filePath in _fileProvider.EnumerateFiles(directoryPath, pattern))
			{
				using var streamReader = new StreamReader(filePath);
				await localizationService.ImportResourcesFromXmlAsync(defaultLanguage, streamReader);
			}

			if (cultureInfo == null || regionInfo == null || cultureInfo.Name == CommonServicesDefaults.DefaultLanguageCulture)
				return;

			//Install resources for user culture
			var userLanguage = new Language
			{
				Name = cultureInfo.NativeName,
				LanguageCulture = cultureInfo.Name,
				UniqueSeoCode = cultureInfo.TwoLetterISOLanguageName,
				FlagImageFileName = $"{regionInfo.TwoLetterISORegionName.ToLowerInvariant()}.png",
				Rtl = cultureInfo.TextInfo.IsRightToLeft,
				Published = true,
				DisplayOrder = 2
			};
			await InsertInstallationDataAsync(userLanguage);

			if (!string.IsNullOrEmpty(languagePackInfo.languagePackDownloadLink))
			{
				//download and import language pack
				try
				{
					var httpClientFactory = EngineContext.Current.Resolve<IHttpClientFactory>();
					var httpClient = httpClientFactory.CreateClient(HttpDefaults.DefaultHttpClient);
					await using var stream = await httpClient.GetStreamAsync(languagePackInfo.languagePackDownloadLink);
					using var streamReader = new StreamReader(stream);
					await localizationService.ImportResourcesFromXmlAsync(userLanguage, streamReader);

					//set this language as default
					userLanguage.DisplayOrder = 0;
					await UpdateInstallationDataAsync(userLanguage);

					//save progress for showing in admin panel (only for first start)
					await InsertInstallationDataAsync(new GenericAttribute
					{
						EntityId = userLanguage.Id,
						Key = CommonServicesDefaults.LanguagePackProgressAttribute,
						KeyGroup = nameof(Language),
						Value = languagePackInfo.languagePackProgress.ToString(),
						//NodeId = 0,
						CreatedOrUpdatedDateUTC = DateTime.UtcNow
					});
				}
				catch { }
			}
			else
			{
				//Install locale resources for default culture
				var patternLocale = $"*.{InstallationDefaults.LocalizationResourcesFileExtension.Split('.')[0]}.{cultureInfo.Name}.{InstallationDefaults.LocalizationResourcesFileExtension.Split('.')[1]}";
				foreach (var filePath in _fileProvider.EnumerateFiles(directoryPath, patternLocale))
				{
					using var streamReader = new StreamReader(filePath);
					await localizationService.ImportResourcesFromXmlAsync(userLanguage, streamReader);
				}
			}
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallCurrenciesAsync(CultureInfo cultureInfo, RegionInfo regionInfo)
		{
			//set some currencies with a rate against the USD
			var defaultCurrencies = new List<string>() { "USD", "AUD", "GBP", "CAD", "CNY", "EUR", "HKD", "JPY", "RUB", "SEK", "INR" };
			var currencies = new List<Currency>
			{
				new() {
					Name = "US Dollar",
					CurrencyCode = "USD",
					Rate = 1,
					DisplayLocale = "en-US",
					CustomFormatting = string.Empty,
					Published = true,
					DisplayOrder = 1,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Australian Dollar",
					CurrencyCode = "AUD",
					Rate = 1.34M,
					DisplayLocale = "en-AU",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 2,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "British Pound",
					CurrencyCode = "GBP",
					Rate = 0.75M,
					DisplayLocale = "en-GB",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 3,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Canadian Dollar",
					CurrencyCode = "CAD",
					Rate = 1.32M,
					DisplayLocale = "en-CA",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 4,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Chinese Yuan Renminbi",
					CurrencyCode = "CNY",
					Rate = 6.43M,
					DisplayLocale = "zh-CN",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 5,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Euro",
					CurrencyCode = "EUR",
					Rate = 0.86M,
					DisplayLocale = string.Empty,
					CustomFormatting = $"{"\u20ac"}0.00", //euro symbol
					Published = false,
					DisplayOrder = 6,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Hong Kong Dollar",
					CurrencyCode = "HKD",
					Rate = 7.84M,
					DisplayLocale = "zh-HK",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 7,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Japanese Yen",
					CurrencyCode = "JPY",
					Rate = 110.45M,
					DisplayLocale = "ja-JP",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 8,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Russian Rouble",
					CurrencyCode = "RUB",
					Rate = 63.25M,
					DisplayLocale = "ru-RU",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 9,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				},
				new() {
					Name = "Swedish Krona",
					CurrencyCode = "SEK",
					Rate = 8.80M,
					DisplayLocale = "sv-SE",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 10,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding1
				},
				new() {
					Name = "Indian Rupee",
					CurrencyCode = "INR",
					Rate = 68.03M,
					DisplayLocale = "en-IN",
					CustomFormatting = string.Empty,
					Published = false,
					DisplayOrder = 12,
					CreatedOnUtc = DateTime.UtcNow,
					UpdatedOnUtc = DateTime.UtcNow,
					RoundingType = RoundingType.Rounding001
				}
			};

			//set additional currency
			if (cultureInfo != null && regionInfo != null)
			{
				if (!defaultCurrencies.Contains(regionInfo.ISOCurrencySymbol))
				{
					currencies.Add(new Currency
					{
						Name = regionInfo.CurrencyEnglishName,
						CurrencyCode = regionInfo.ISOCurrencySymbol,
						Rate = 1,
						DisplayLocale = cultureInfo.Name,
						CustomFormatting = string.Empty,
						Published = true,
						DisplayOrder = 0,
						CreatedOnUtc = DateTime.UtcNow,
						UpdatedOnUtc = DateTime.UtcNow,
						RoundingType = RoundingType.Rounding001
					});
				}

				foreach (var currency in currencies.Where(currency => currency.CurrencyCode == regionInfo.ISOCurrencySymbol))
				{
					currency.Published = true;
					currency.DisplayOrder = 0;
				}
			}

			await InsertInstallationDataAsync(currencies);
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallSettingsAsync(RegionInfo regionInfo)
		{
			var isMetric = regionInfo?.IsMetric ?? false;
			var country = regionInfo?.TwoLetterISORegionName ?? string.Empty;
			var isGermany = country == "DE";
			var isEurope = ISO3166.FromCountryCode(country)?.SubjectToVat ?? false;

			var settingService = EngineContext.Current.Resolve<ISettingService>();

			await settingService.SaveSettingAsync(new CommonSettings
			{
				LogAllErrors = true,
				RestartTimeout = CommonServicesDefaults.RestartTimeout,
			});

			await settingService.SaveSettingAsync(new ClusteringSettings
			{
				IgnoreAcl = true,
				IgnoreNodeLimitations = true,
				//UNDONE MORE CLUSTERING SETTINGS
			});

			await settingService.SaveSettingAsync(new LocalizationSettings
			{
				DefaultAdminLanguageId = _languageRepository.Table.Single(l => l.LanguageCulture == CommonServicesDefaults.DefaultLanguageCulture).Id,
				UseImagesForLanguageSelection = false,
				SeoFriendlyUrlsForLanguagesEnabled = false,
				AutomaticallyDetectLanguage = false,
				LoadAllLocaleRecordsOnStartup = true,
				LoadAllLocalizedPropertiesOnStartup = true,
				LoadAllVirtualFileRecordsOnStartup = false,
				IgnoreRtlPropertyForAdminArea = false
			});

			await settingService.SaveSettingAsync(new UserSettings
			{
				UsernamesEnabled = true,
				CheckUsernameAvailabilityEnabled = true,
				AllowUsersToChangeUsernames = true,
				DefaultPasswordFormat = PasswordFormat.Hashed,
				HashedPasswordFormat = UserServicesDefaults.DefaultHashedPasswordFormat,
				PasswordMinLength = 6,
				PasswordMaxLength = 64,
				PasswordRequireDigit = false,
				PasswordRequireLowercase = false,
				PasswordRequireNonAlphanumeric = false,
				PasswordRequireUppercase = false,
				UnduplicatedPasswordsNumber = 4,
				PasswordRecoveryLinkDaysValid = 7,
				PasswordLifetime = 90,
				FailedPasswordAllowedAttempts = 0,
				FailedPasswordLockoutMinutes = 30,
				AllowUsersToUploadAvatars = true,
				AvatarMaximumSizeBytes = 20000,
				DefaultAvatarEnabled = true,
				ShowUsersLocation = true,
				ShowUsersJoinDate = true,
				AllowViewingProfiles = true,
				NotifyNewUserRegistration = true,
				DownloadableProductsValidateUser = false,
				OnlineUserMinutes = 20,
				StoreIpAddresses = true,
				LastActivityMinutes = 15,
				SuffixDeletedUsers = false,
				DeleteGuestTaskOlderThanMinutes = 1440,
			});

			var primaryCurrency = "EUR";
			await settingService.SaveSettingAsync(new CurrencySettings
			{
				DisplayCurrencyLabel = false,
				PrimaryNodeCurrencyId =
					_currencyRepository.Table.Single(c => c.CurrencyCode == primaryCurrency).Id,
				PrimaryExchangeRateCurrencyId =
					_currencyRepository.Table.Single(c => c.CurrencyCode == primaryCurrency).Id,
				ActiveExchangeRateProviderSystemName = "ExchangeRate.ECB",
				AutoUpdateEnabled = false
			});

			var baseDimension = isMetric ? SystemKeywords.Measures.Meters : SystemKeywords.Measures.Inches;
			var baseWeight = isMetric ? SystemKeywords.Measures.Kilograms : SystemKeywords.Measures.Pounds;

			await settingService.SaveSettingAsync(new MeasureSettings
			{
				BaseDimensionId =
					_measureDimensionRepository.Table.Single(m => m.SystemKeyword == baseDimension).Id,
				BaseWeightId = _measureWeightRepository.Table.Single(m => m.SystemKeyword == baseWeight).Id
			});

			await settingService.SaveSettingAsync(new SecuritySettings
			{
				EncryptionKey = CommonHelper.GenerateRandomDigitCode(16),
				AdminAllowedIpAddresses = null,
			});

			await settingService.SaveSettingAsync(new DateTimeSettings
			{
				DefaultNodeTimeZoneId = string.Empty,
				AllowUsersToSetTimeZone = true
			});

			await settingService.SaveSettingAsync(new ProxySettings
			{
				Enabled = false,
				Address = string.Empty,
				Port = string.Empty,
				Username = string.Empty,
				Password = string.Empty,
				BypassOnLocal = true,
				PreAuthenticate = true
			});
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallUsersAndRolesAsync(string defaultUserEmail, string defaultUserPassword)
		{
			var urAdministrators = new UserRole
			{
				Name = "Administrators",
				Active = true,
				IsSystemRole = true,
				SystemName = UserDefaults.AdministratorsRoleName
			};
			var urModerators = new UserRole
			{
				Name = "Moderators",
				Active = true,
				IsSystemRole = true,
				SystemName = UserDefaults.ModeratorsRoleName
			};
			var urRegistered = new UserRole
			{
				Name = "Registered",
				Active = true,
				IsSystemRole = true,
				SystemName = UserDefaults.RegisteredRoleName
			};
			var urGuests = new UserRole
			{
				Name = "Guests",
				Active = true,
				IsSystemRole = true,
				SystemName = UserDefaults.GuestsRoleName
			};
			var urPartners = new UserRole
			{
				Name = "Partners",
				Active = true,
				IsSystemRole = true,
				SystemName = UserDefaults.PartnersRoleName
			};
			var urPlayers = new UserRole
			{
				Name = "Players",
				Active = true,
				IsSystemRole = true,
				SystemName = UserDefaults.PlayersRoleName
			};
			var userRoles = new List<UserRole>
			{
				urAdministrators,
				urModerators,
				urRegistered,
				urGuests,
				urPartners,
				urPlayers
			};

			await InsertInstallationDataAsync(userRoles);

			//default node 
			var defaultNode = await _nodeRepository.Table.FirstOrDefaultAsync() ?? throw new NodeException("No default node could be loaded");
			var nodeId = defaultNode.Id;

			//admin user
			var adminUser = new User
			{
				UserGuid = Guid.NewGuid(),
				Email = defaultUserEmail,
				Username = defaultUserEmail,
				Active = true,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInNodeId = nodeId
			};

			//var defaultAdminUserAddress = await InsertInstallationDataAsync(
			//	new Address
			//	{
			//		FirstName = "Administrador",
			//		LastName = string.Empty,
			//		PhoneNumber = string.Empty,
			//		Email = defaultUserEmail,
			//		FaxNumber = string.Empty,
			//		Company = "Dragon Corp. Games S.L.",
			//		Address1 = string.Empty,
			//		Address2 = string.Empty,
			//		City = "Barcelona",
			//		StateProvinceId = _stateProvinceRepository.Table.FirstOrDefault(sp => sp.Name == "Barcelona")?.Id,
			//		CountryId = _countryRepository.Table.FirstOrDefault(c => c.ThreeLetterIsoCode == "ESP")?.Id,
			//		ZipPostalCode = "08080",
			//		CreatedOnUtc = DateTime.UtcNow
			//	});

			//adminUser.BillingAddressId = defaultAdminUserAddress.Id;
			//adminUser.ShippingAddressId = defaultAdminUserAddress.Id;
			//adminUser.FirstName = defaultAdminUserAddress.FirstName;
			//adminUser.LastName = defaultAdminUserAddress.LastName;

			await InsertInstallationDataAsync(adminUser);

			await InsertInstallationDataAsync(
				new UserUserRoleMapping { UserId = adminUser.Id, UserRoleId = urAdministrators.Id },
				new UserUserRoleMapping { UserId = adminUser.Id, UserRoleId = urModerators.Id },
				new UserUserRoleMapping { UserId = adminUser.Id, UserRoleId = urRegistered.Id });

			//set hashed admin password
			var userService = EngineContext.Current.Resolve<IUserService>();
			var encryptionService = EngineContext.Current.Resolve<IEncryptionService>();

			//at this point request is valid
			var userPassword = new UserPassword
			{
				UserId = adminUser.Id,
				PasswordFormat = PasswordFormat.Hashed,
				CreatedOnUtc = DateTime.UtcNow
			};
			var saltKey = encryptionService.CreateSaltKey(UserServicesDefaults.PasswordSaltKeySize);
			userPassword.PasswordSalt = saltKey;
			userPassword.Password = encryptionService.CreatePasswordHash(defaultUserPassword, saltKey, UserServicesDefaults.DefaultHashedPasswordFormat);

			await userService.InsertUserPasswordAsync(userPassword);

			//search engine (crawler) built-in user
			var searchEngineUser = new User
			{
				Email = "builtin@search_engine.com",
				UserGuid = Guid.NewGuid(),
				AdminComment = "Built-in system guest record used for requests from search engines.",
				Active = true,
				IsSystemAccount = true,
				SystemName = UserDefaults.SearchEngineUserName,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInNodeId = nodeId
			};

			await InsertInstallationDataAsync(searchEngineUser);

			await InsertInstallationDataAsync(new UserUserRoleMapping { UserRoleId = urGuests.Id, UserId = searchEngineUser.Id });

			//built-in user for background tasks
			var backgroundTaskUser = new User
			{
				Email = "builtin@background-task.com",
				UserGuid = Guid.NewGuid(),
				AdminComment = "Built-in system record used for background tasks.",
				Active = true,
				IsSystemAccount = true,
				SystemName = UserDefaults.BackgroundTaskUserName,
				CreatedOnUtc = DateTime.UtcNow,
				LastActivityDateUtc = DateTime.UtcNow,
				RegisteredInNodeId = nodeId
			};

			await InsertInstallationDataAsync(backgroundTaskUser);

			await InsertInstallationDataAsync(new UserUserRoleMapping { UserId = backgroundTaskUser.Id, UserRoleId = urGuests.Id });
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallActivityLogTypesAsync()
		{
			var activityLogTypes = new List<ActivityLogType>
			{
				//admin area activities
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewAddressAttribute,
					Enabled = true,
					Name = "Add a new address attribute"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewAddressAttributeValue,
					Enabled = true,
					Name = "Add a new address attribute value"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewCampaign,
					Enabled = true,
					Name = "Add a new campaign"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewCountry,
					Enabled = true,
					Name = "Add a new country"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewCurrency,
					Enabled = true,
					Name = "Add a new currency"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewUser,
					Enabled = true,
					Name = "Add a new user"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewUserAttribute,
					Enabled = true,
					Name = "Add a new user attribute"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewUserAttributeValue,
					Enabled = true,
					Name = "Add a new user attribute value"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewUserRole,
					Enabled = true,
					Name = "Add a new user role"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewEmailAccount,
					Enabled = true,
					Name = "Add a new email account"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewLanguage,
					Enabled = true,
					Name = "Add a new language"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewMeasureDimension,
					Enabled = true,
					Name = "Add a new measure dimension"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewMeasureWeight,
					Enabled = true,
					Name = "Add a new measure weight"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewSetting,
					Enabled = true,
					Name = "Add a new setting"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewStateProvince,
					Enabled = true,
					Name = "Add a new state or province"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.AddNewNode,
					Enabled = true,
					Name = "Add a new node"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteActivityLog,
					Enabled = true,
					Name = "Delete activity log"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteAddressAttribute,
					Enabled = true,
					Name = "Delete an address attribute"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteAddressAttributeValue,
					Enabled = true,
					Name = "Delete an address attribute value"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteCampaign,
					Enabled = true,
					Name = "Delete a campaign"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteCountry,
					Enabled = true,
					Name = "Delete a country"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteCurrency,
					Enabled = true,
					Name = "Delete a currency"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteUser,
					Enabled = true,
					Name = "Delete a user"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteUserAttribute,
					Enabled = true,
					Name = "Delete a user attribute"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteUserAttributeValue,
					Enabled = true,
					Name = "Delete a user attribute value"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteUserRole,
					Enabled = true,
					Name = "Delete a user role"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteEmailAccount,
					Enabled = true,
					Name = "Delete an email account"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteLanguage,
					Enabled = true,
					Name = "Delete a language"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteMeasureDimension,
					Enabled = true,
					Name = "Delete a measure dimension"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteMeasureWeight,
					Enabled = true,
					Name = "Delete a measure weight"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteMessageTemplate,
					Enabled = true,
					Name = "Delete a message template"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteModule,
					Enabled = true,
					Name = "Delete a module"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteSetting,
					Enabled = true,
					Name = "Delete a setting"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteStateProvince,
					Enabled = true,
					Name = "Delete a state or province"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteNode,
					Enabled = true,
					Name = "Delete a node"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.DeleteSystemLog,
					Enabled = true,
					Name = "Delete system log"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditActivityLogTypes,
					Enabled = true,
					Name = "Edit activity log types"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditAddressAttribute,
					Enabled = true,
					Name = "Edit an address attribute"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditAddressAttributeValue,
					Enabled = true,
					Name = "Edit an address attribute value"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditCampaign,
					Enabled = true,
					Name = "Edit a campaign"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditCountry,
					Enabled = true,
					Name = "Edit a country"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditCurrency,
					Enabled = true,
					Name = "Edit a currency"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditUser,
					Enabled = true,
					Name = "Edit a user"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditUserAttribute,
					Enabled = true,
					Name = "Edit a user attribute"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditUserAttributeValue,
					Enabled = true,
					Name = "Edit a user attribute value"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditUserRole,
					Enabled = true,
					Name = "Edit a user role"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditEmailAccount,
					Enabled = true,
					Name = "Edit an email account"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditLanguage,
					Enabled = true,
					Name = "Edit a language"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditMeasureDimension,
					Enabled = true,
					Name = "Edit a measure dimension"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditMeasureWeight,
					Enabled = true,
					Name = "Edit a measure weight"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditMessageTemplate,
					Enabled = true,
					Name = "Edit a message template"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditModule,
					Enabled = true,
					Name = "Edit a module"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditSettings,
					Enabled = true,
					Name = "Edit setting(s)"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditStateProvince,
					Enabled = true,
					Name = "Edit a state or province"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditNode,
					Enabled = true,
					Name = "Edit a node"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.EditTask,
					Enabled = true,
					Name = "Edit a task"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.ImpersonationStarted,
					Enabled = true,
					Name = "User impersonation session. Started"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.ImpersonationFinished,
					Enabled = true,
					Name = "User impersonation session. Finished"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.ImportNewsLetterSubscriptions,
					Enabled = true,
					Name = "Newsletter subscriptions were imported"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.ImportStates,
					Enabled = true,
					Name = "States were imported"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.ExportUsers,
					Enabled = true,
					Name = "Users were exported"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.ExportStates,
					Enabled = true,
					Name = "States were exported"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.ExportNewsLetterSubscriptions,
					Enabled = true,
					Name = "Newsletter subscriptions were exported"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.InstallNewModule,
					Enabled = true,
					Name = "Install a new module"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.UninstallModule,
					Enabled = true,
					Name = "Uninstall a module"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.UploadNewModule,
					Enabled = true,
					Name = "Upload a module"
				},
				new() {
					SystemKeyword = SystemKeywords.AdminArea.UploadIcons,
					Enabled = true,
					Name = "Upload a favicon and app icons"
				},
				//public node activities
				new() {
					SystemKeyword = SystemKeywords.PublicServer.ContactUs,
					Enabled = false,
					Name = "Public node. Use contact us form"
				},
				new() {
					SystemKeyword = SystemKeywords.PublicServer.Login,
					Enabled = false,
					Name = "Public node. Login"
				},
				new() {
					SystemKeyword = SystemKeywords.PublicServer.Logout,
					Enabled = false,
					Name = "Public node. Logout"
				},
			};

			await InsertInstallationDataAsync(activityLogTypes);
		}

		/// <returns>A task that represents the asynchronous operation</returns>
		protected virtual async Task InstallScheduleTasksAsync()
		{
			var lastEnabledUtc = DateTime.UtcNow;
			var tasks = new List<ScheduleTask>
			{
				new() {
					Name = "Send emails",
					//1 minute
					Seconds = 60,
					Type = "ARWNI2S.Node.Services.Messages.QueuedMessagesSendTask, ARWNI2S.Node.Services",
					Enabled = true,
					LastEnabledUtc = lastEnabledUtc,
					StopOnError = false
				},
				new() {
					Name = "Keep alive",
					//5 minutes
					Seconds = 300,
					Type = "ARWNI2S.Node.Services.Common.KeepAliveTask, ARWNI2S.Node.Services",
					Enabled = true,
					LastEnabledUtc = lastEnabledUtc,
					StopOnError = false
				},
				new() {
					Name = "Delete guests",
					//10 minutes
					Seconds = 600,
					Type = "ARWNI2S.Node.Services.Users.DeleteGuestsTask, ARWNI2S.Node.Services",
					Enabled = true,
					LastEnabledUtc = lastEnabledUtc,
					StopOnError = false
				},
				new() {
					Name = "Clear cache",
					//10 minutes
					Seconds = 600,
					Type = "ARWNI2S.Node.Services.Caching.ClearCacheTask, ARWNI2S.Node.Services",
					Enabled = false,
					StopOnError = false
				},
				new() {
					Name = "Clear log",
					//60 minutes
					Seconds = 3600,
					Type = "ARWNI2S.Node.Services.Logging.ClearLogTask, ARWNI2S.Node.Services",
					Enabled = false,
					StopOnError = false
				},
				new() {
					Name = "Update currency exchange rates",
					//60 minutes
					Seconds = 3600,
					Type = "ARWNI2S.Node.Services.Directory.UpdateExchangeRateTask, ARWNI2S.Node.Services",
					Enabled = true,
					LastEnabledUtc = lastEnabledUtc,
					StopOnError = false
				},
				new() {
					Name = "Delete inactive users (GDPR)",
					//24 hours
					Seconds = 86400,
					Type = "ARWNI2S.Node.Services.Gdpr.DeleteInactiveUsersTask, ARWNI2S.Node.Services",
					Enabled = false,
					StopOnError = false
				},
				new() {
					Name = "Update blocks (Web3)",
					//10 minutes
					Seconds = 600,
					Type = "ARWNI2S.Node.Services.Metalink.UpdateBlockchainBlocksTask, ARWNI2S.Node.Services",
					Enabled = false,
					StopOnError = false
				}
			};

			await InsertInstallationDataAsync(tasks);
		}

		#endregion

		#region Methods

		/// <summary>
		/// Install required data
		/// </summary>
		/// <param name="defaultUserEmail">Default user email</param>
		/// <param name="defaultUserPassword">Default user password</param>
		/// <param name="languagePackInfo">Language pack info</param>
		/// <param name="regionInfo">RegionInfo</param>
		/// <param name="cultureInfo">CultureInfo</param>
		/// <returns>A task that represents the asynchronous operation</returns>
		public virtual async Task InstallRequiredDataAsync(string defaultUserEmail, string defaultUserPassword,
			(string languagePackDownloadLink, int languagePackProgress) languagePackInfo, RegionInfo regionInfo, CultureInfo cultureInfo)
		{
			await InstallNodeAsync();
			await InstallMeasuresAsync(regionInfo);
			await InstallLanguagesAsync(languagePackInfo, cultureInfo, regionInfo);
			await InstallCurrenciesAsync(cultureInfo, regionInfo);
			await InstallSettingsAsync(regionInfo);
			await InstallUsersAndRolesAsync(defaultUserEmail, defaultUserPassword);
			await InstallActivityLogTypesAsync();
			await InstallScheduleTasksAsync();
		}

		public virtual async Task InstallNodeAsync(string adminUserEmail, string passwordUserPassword)
		{
			//set hashed admin password
			var userService = EngineContext.Current.Resolve<IUserService>();

			var admin = userService.GetUserByUsernameAsync(adminUserEmail)
				?? userService.GetUserByEmailAsync(adminUserEmail) ?? throw new NodeException($"Not found: {adminUserEmail}");



			await InstallNodeAsync();
		}

		#endregion
	}
}

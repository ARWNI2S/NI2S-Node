using ARWNI2S.Caching;
using ARWNI2S.Collections;
using ARWNI2S.Configuration;
using ARWNI2S.Core.Caching;
using ARWNI2S.Core.Common;
using ARWNI2S.Core.Configuration;
using ARWNI2S.Core.Events;
using ARWNI2S.Core.Installation;
using ARWNI2S.Core.Logging;
using ARWNI2S.Core.Plugins;
using ARWNI2S.Engine;
using ARWNI2S.Engine.Builder;
using ARWNI2S.Events;
using ARWNI2S.Infrastructure;
using ARWNI2S.Plugins;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Core
{
    public class CommonStartup : INiisStartup
    {
        public int Order => StartupStage.CommonStartup;

        public void ConfigureEngine(IEngineBuilder engineBuilder)
        {

        }

        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            //file provider
            services.AddScoped<INiisFileProvider, NI2SFileProvider>();

            ////web helper
            //services.AddScoped<IWebHelper, WebHelper>();

            ////user agent helper
            //services.AddScoped<IUserAgentHelper, UserAgentHelper>();

            //plugins
            services.AddScoped<IPluginService, PluginService>();
            //services.AddScoped<PluginFeedManager>();

            //static cache manager
            var niisSettings = Singleton<NI2SSettings>.Instance;
            var distributedCacheConfig = niisSettings.Get<DistributedCacheConfig>();

            services.AddTransient(typeof(IConcurrentCollection<>), typeof(ConcurrentTrie<>));

            services.AddSingleton<ICacheKeyManager, CacheKeyManager>();
            services.AddScoped<IShortTermCacheManager, PerFrameCacheManager>();

            if (distributedCacheConfig.Enabled)
            {
                switch (distributedCacheConfig.DistributedCacheType)
                {
                    case DistributedCacheType.Memory:
                        services.AddScoped<IStaticCacheManager, MemoryDistributedCacheManager>();
                        services.AddScoped<ICacheKeyService, MemoryDistributedCacheManager>();
                        break;
                    case DistributedCacheType.SqlServer:
                        services.AddScoped<IStaticCacheManager, MsSqlServerCacheManager>();
                        services.AddScoped<ICacheKeyService, MsSqlServerCacheManager>();
                        break;
                    case DistributedCacheType.Redis:
                        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                        services.AddScoped<IStaticCacheManager, RedisCacheManager>();
                        services.AddScoped<ICacheKeyService, RedisCacheManager>();
                        break;
                    case DistributedCacheType.RedisSynchronizedMemory:
                        services.AddSingleton<IRedisConnectionWrapper, RedisConnectionWrapper>();
                        services.AddSingleton<ISynchronizedMemoryCache, RedisSynchronizedMemoryCache>();
                        services.AddSingleton<IStaticCacheManager, SynchronizedMemoryCacheManager>();
                        services.AddScoped<ICacheKeyService, SynchronizedMemoryCacheManager>();
                        break;
                }

                services.AddSingleton<ILocker, DistributedCacheLocker>();
            }
            else
            {
                services.AddSingleton<ILocker, MemoryCacheLocker>();
                services.AddSingleton<IStaticCacheManager, MemoryCacheManager>();
                services.AddScoped<ICacheKeyService, MemoryCacheManager>();
            }

            ////work context
            //services.AddScoped<IWorkContext, WebWorkContext>();

            ////store context
            //services.AddScoped<IStoreContext, WebStoreContext>();

            //services
            //services.AddScoped<IBackInStockSubscriptionService, BackInStockSubscriptionService>();
            //services.AddScoped<ICategoryService, CategoryService>();
            //services.AddScoped<ICompareProductsService, CompareProductsService>();
            //services.AddScoped<IRecentlyViewedProductsService, RecentlyViewedProductsService>();
            //services.AddScoped<IManufacturerService, ManufacturerService>();
            //services.AddScoped<IPriceFormatter, PriceFormatter>();
            //services.AddScoped<IProductAttributeFormatter, ProductAttributeFormatter>();
            //services.AddScoped<IProductAttributeParser, ProductAttributeParser>();
            //services.AddScoped<IProductAttributeService, ProductAttributeService>();
            //services.AddScoped<IProductService, ProductService>();
            //services.AddScoped<ICopyProductService, CopyProductService>();
            //services.AddScoped<ISpecificationAttributeService, SpecificationAttributeService>();
            //services.AddScoped<IProductTemplateService, ProductTemplateService>();
            //services.AddScoped<ICategoryTemplateService, CategoryTemplateService>();
            //services.AddScoped<IManufacturerTemplateService, ManufacturerTemplateService>();
            //services.AddScoped<ITopicTemplateService, TopicTemplateService>();
            //services.AddScoped<IProductTagService, ProductTagService>();
            //services.AddScoped<IAddressService, AddressService>();
            //services.AddScoped<IAffiliateService, AffiliateService>();
            //services.AddScoped<IVendorService, VendorService>();
            //services.AddScoped<ISearchTermService, SearchTermService>();

            services.AddScoped<IGenericAttributeService, GenericAttributeService>();

            //services.AddScoped<IMaintenanceService, MaintenanceService>();
            //services.AddScoped<ICustomerService, CustomerService>();
            //services.AddScoped<ICustomerRegistrationService, CustomerRegistrationService>();
            //services.AddScoped<ICustomerReportService, CustomerReportService>();
            //services.AddScoped<IPermissionService, PermissionService>();
            //services.AddScoped<IAclService, AclService>();
            //services.AddScoped<IPriceCalculationService, PriceCalculationService>();
            //services.AddScoped<IGeoLookupService, GeoLookupService>();
            //services.AddScoped<ICountryService, CountryService>();
            //services.AddScoped<ICurrencyService, CurrencyService>();
            //services.AddScoped<IMeasureService, MeasureService>();
            //services.AddScoped<IStateProvinceService, StateProvinceService>();
            //services.AddScoped<IStoreService, StoreService>();
            //services.AddScoped<IStoreMappingService, StoreMappingService>();
            //services.AddScoped<IDiscountService, DiscountService>();
            //services.AddScoped<ILocalizationService, LocalizationService>();
            //services.AddScoped<ILocalizedEntityService, LocalizedEntityService>();
            //services.AddScoped<ILanguageService, LanguageService>();
            //services.AddScoped<IDownloadService, DownloadService>();
            //services.AddScoped<IMessageTemplateService, MessageTemplateService>();
            //services.AddScoped<IQueuedEmailService, QueuedEmailService>();
            //services.AddScoped<INewsLetterSubscriptionService, NewsLetterSubscriptionService>();
            //services.AddScoped<INotificationService, NotificationService>();
            //services.AddScoped<ICampaignService, CampaignService>();
            //services.AddScoped<IEmailAccountService, EmailAccountService>();
            //services.AddScoped<IWorkflowMessageService, WorkflowMessageService>();
            //services.AddScoped<IMessageTokenProvider, MessageTokenProvider>();
            //services.AddScoped<ITokenizer, Tokenizer>();
            //services.AddScoped<ISmtpBuilder, SmtpBuilder>();
            //services.AddScoped<IEmailSender, EmailSender>();
            //services.AddScoped<ICheckoutAttributeFormatter, CheckoutAttributeFormatter>();
            //services.AddScoped<IGiftCardService, GiftCardService>();
            //services.AddScoped<IOrderService, OrderService>();
            //services.AddScoped<IOrderReportService, OrderReportService>();
            //services.AddScoped<IOrderProcessingService, OrderProcessingService>();
            //services.AddScoped<IOrderTotalCalculationService, OrderTotalCalculationService>();
            //services.AddScoped<IReturnRequestService, ReturnRequestService>();
            //services.AddScoped<IRewardPointService, RewardPointService>();
            //services.AddScoped<IShoppingCartService, ShoppingCartService>();
            //services.AddScoped<ICustomNumberFormatter, CustomNumberFormatter>();
            //services.AddScoped<IPaymentService, PaymentService>();
            //services.AddScoped<IEncryptionService, EncryptionService>();
            //services.AddScoped<IAuthenticationService, CookieAuthenticationService>();
            //services.AddScoped<IUrlRecordService, UrlRecordService>();
            //services.AddScoped<IShipmentService, ShipmentService>();
            //services.AddScoped<IShippingService, ShippingService>();
            //services.AddScoped<IDateRangeService, DateRangeService>();
            //services.AddScoped<ITaxCategoryService, TaxCategoryService>();
            //services.AddScoped<ICheckVatService, CheckVatService>();
            //services.AddScoped<ITaxService, TaxService>();

            services.AddScoped<ILogService, NodeDbLogger>();

            //services.AddScoped<ICustomerActivityService, CustomerActivityService>();
            //services.AddScoped<IForumService, ForumService>();
            //services.AddScoped<IGdprService, GdprService>();
            //services.AddScoped<IPollService, PollService>();
            //services.AddScoped<IBlogService, BlogService>();
            //services.AddScoped<ITopicService, TopicService>();
            //services.AddScoped<INewsService, NewsService>();
            //services.AddScoped<IDateTimeHelper, DateTimeHelper>();
            //services.AddScoped<INopHtmlHelper, NopHtmlHelper>();
            //services.AddScoped<IScheduleTaskService, ScheduleTaskService>();
            //services.AddScoped<IExportManager, ExportManager>();
            //services.AddScoped<IImportManager, ImportManager>();
            //services.AddScoped<IPdfService, PdfService>();
            //services.AddScoped<IUploadService, UploadService>();
            //services.AddScoped<IThemeProvider, ThemeProvider>();
            //services.AddScoped<IThemeContext, ThemeContext>();
            //services.AddScoped<IExternalAuthenticationService, ExternalAuthenticationService>();
            //services.AddSingleton<IRoutePublisher, RoutePublisher>();
            //services.AddScoped<IReviewTypeService, ReviewTypeService>();

            services.AddSingleton<IEventPublisher, EventPublisher>();
            services.AddScoped<ISettingService, SettingService>();

            //services.AddScoped<IBBCodeHelper, BBCodeHelper>();
            //services.AddScoped<IHtmlFormatter, HtmlFormatter>();
            //services.AddScoped<IVideoService, VideoService>();
            //services.AddScoped<INopUrlHelper, NopUrlHelper>();
            //services.AddScoped<IWidgetModelFactory, WidgetModelFactory>();

            ////attribute services
            //services.AddScoped(typeof(IAttributeService<,>), typeof(AttributeService<,>));

            ////attribute parsers
            //services.AddScoped(typeof(IAttributeParser<,>), typeof(Services.Attributes.AttributeParser<,>));

            ////attribute formatter
            //services.AddScoped(typeof(IAttributeFormatter<,>), typeof(AttributeFormatter<,>));

            //plugin managers
            services.AddScoped(typeof(IPluginManager<>), typeof(PluginManager<>));
            //services.AddScoped<IAuthenticationPluginManager, AuthenticationPluginManager>();
            //services.AddScoped<IMultiFactorAuthenticationPluginManager, MultiFactorAuthenticationPluginManager>();
            //services.AddScoped<IWidgetPluginManager, WidgetPluginManager>();
            //services.AddScoped<IExchangeRatePluginManager, ExchangeRatePluginManager>();
            //services.AddScoped<IDiscountPluginManager, DiscountPluginManager>();
            //services.AddScoped<IPaymentPluginManager, PaymentPluginManager>();
            //services.AddScoped<IPickupPluginManager, PickupPluginManager>();
            //services.AddScoped<IShippingPluginManager, ShippingPluginManager>();
            //services.AddScoped<ITaxPluginManager, TaxPluginManager>();
            //services.AddScoped<ISearchPluginManager, SearchPluginManager>();

            //services.AddSingleton<IActionContextAccessor, ActionContextAccessor>();

            //register all settings
            var typeFinder = Singleton<ITypeFinder>.Instance;

            //var settings = typeFinder.FindClassesOfType(typeof(ISettings), false).ToList();
            //foreach (var setting in settings)
            //{
            //    services.AddScoped(setting, serviceProvider =>
            //    {
            //        var storeId = DataSettingsManager.IsDatabaseInstalled()
            //            ? serviceProvider.GetRequiredService<IStoreContext>().GetCurrentStore()?.Id ?? 0
            //            : 0;

            //        return serviceProvider.GetRequiredService<ISettingService>().LoadSettingAsync(setting, storeId).Result;
            //    });
            //}

            ////picture service
            //if (niisSettings.Get<AzureBlobConfig>().Enabled)
            //    services.AddScoped<IPictureService, AzurePictureService>();
            //else
            //    services.AddScoped<IPictureService, PictureService>();

            ////roxy file manager
            //services.AddScoped<IRoxyFilemanService, RoxyFilemanService>();
            //services.AddScoped<IRoxyFilemanFileProvider, RoxyFilemanFileProvider>();

            //installation service
            services.AddScoped<IInstallationService, InstallationService>();

            ////slug route transformer
            //if (DataSettingsManager.IsDatabaseInstalled())
            //    services.AddScoped<SlugRouteTransformer>();

            ////schedule tasks
            //services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();

            //event consumers
            var consumers = typeFinder.FindClassesOfType(typeof(IConsumer<>)).ToList();
            foreach (var consumer in consumers)
                foreach (var findInterface in consumer.FindInterfaces((type, criteria) =>
                {
                    var isMatch = type.IsGenericType && ((Type)criteria).IsAssignableFrom(type.GetGenericTypeDefinition());
                    return isMatch;
                }, typeof(IConsumer<>)))
                    services.AddScoped(findInterface, consumer);

            ////admin menu
            //services.AddScoped<IAdminMenu, AdminMenu>();

            //register the Lazy resolver for .Net IoC
            var useAutofac = niisSettings.Get<CommonConfig>().UseAutofac;
            if (!useAutofac)
                services.AddScoped(typeof(Lazy<>), typeof(LazyInstance<>));
        }
    }
}

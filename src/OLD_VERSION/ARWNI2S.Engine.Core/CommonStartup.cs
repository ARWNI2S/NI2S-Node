using ARWNI2S.Core.Events;
using ARWNI2S.Core.Installation;
using ARWNI2S.Core.Plugins;

namespace ARWNI2S.Core
{
    public class CommonStartup : IInitializer
    {
        public int Order => InitStage.Initialization;

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



            ////work context
            //services.AddScoped<IWorkContext, WebWorkContext>();

            ////store context
            //services.AddScoped<IStoreContext, WebStoreContext>();

            //services
            //services.AddScoped<IMaintenanceService, MaintenanceService>();

            //services.AddScoped<IGeoLookupService, GeoLookupService>();
            //services.AddScoped<ICountryService, CountryService>();
            //services.AddScoped<ICurrencyService, CurrencyService>();
            //services.AddScoped<IMeasureService, MeasureService>();
            //services.AddScoped<IStateProvinceService, StateProvinceService>();
            //services.AddScoped<IStoreService, StoreService>();
            //services.AddScoped<IStoreMappingService, StoreMappingService>();

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


            //services.AddScoped<IUserActivityService, UserActivityService>();
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


            ////picture service
            //if (niisSettings.Get<AzureBlobConfig>().Enabled)
            //    services.AddScoped<IPictureService, AzurePictureService>();
            //else
            //    services.AddScoped<IPictureService, PictureService>();

            ////roxy file manager
            //services.AddScoped<IRoxyFilemanService, RoxyFilemanService>();
            //services.AddScoped<IRoxyFilemanFileProvider, RoxyFilemanFileProvider>();


            ////slug route transformer
            //if (DataSettingsManager.IsDatabaseInstalled())
            //    services.AddScoped<SlugRouteTransformer>();

            ////schedule tasks
            //services.AddTransient<IScheduleTaskRunner, ScheduleTaskRunner>();


            ////admin menu
            //services.AddScoped<IAdminMenu, AdminMenu>();

            //register the Lazy resolver for .Net IoC
            var useAutofac = niisSettings.Get<CommonConfig>().UseAutofac;
            if (!useAutofac)
                services.AddScoped(typeof(Lazy<>), typeof(LazyInstance<>));
        }
    }
}

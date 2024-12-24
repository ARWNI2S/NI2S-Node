namespace ARWNI2S.Core.Engine
{
    /// <summary>
    /// Represents extensions of IServiceCollection
    /// </summary>
    public static class EngineServiceCollectionExtensions
    {
        ///// <summary>
        ///// Adds data protection services
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddNopDataProtection(this IServiceCollection services)
        //{
        //    var appSettings = Singleton<NI2SSettings>.Instance;
        //    if (appSettings.Get<AzureBlobConfig>().Enabled && appSettings.Get<AzureBlobConfig>().StoreDataProtectionKeys)
        //    {
        //        var blobServiceClient = new BlobServiceClient(appSettings.Get<AzureBlobConfig>().ConnectionString);
        //        var blobContainerClient = blobServiceClient.GetBlobContainerClient(appSettings.Get<AzureBlobConfig>().DataProtectionKeysContainerName);
        //        var blobClient = blobContainerClient.GetBlobClient(DataProtectionDefaults.AzureDataProtectionKeyFile);

        //        var dataProtectionBuilder = services.AddDataProtection().PersistKeysToAzureBlobStorage(blobClient);

        //        if (!appSettings.Get<AzureBlobConfig>().DataProtectionKeysEncryptWithVault)
        //            return;

        //        var keyIdentifier = appSettings.Get<AzureBlobConfig>().DataProtectionKeysVaultId;
        //        var credentialOptions = new DefaultAzureCredentialOptions();
        //        var tokenCredential = new DefaultAzureCredential(credentialOptions);

        //        dataProtectionBuilder.ProtectKeysWithAzureKeyVault(new Uri(keyIdentifier), tokenCredential);
        //    }
        //    else
        //    {
        //        var dataProtectionKeysPath = CommonHelper.DefaultFileProvider.MapPath(DataProtectionDefaults.DataProtectionKeysPath);
        //        var dataProtectionKeysFolder = new System.IO.DirectoryInfo(dataProtectionKeysPath);

        //        //configure the data protection system to persist keys to the specified directory
        //        services.AddDataProtection().PersistKeysToFileSystem(dataProtectionKeysFolder);
        //    }
        //}

        ///// <summary>
        ///// Add and configure default HTTP clients
        ///// </summary>
        ///// <param name="services">Collection of service descriptors</param>
        //public static void AddNopHttpClients(this IServiceCollection services)
        //{
        //    //default client
        //    services.AddHttpClient(NopHttpDefaults.DefaultHttpClient).WithProxy();

        //    //client to request current store
        //    services.AddHttpClient<StoreHttpClient>();

        //    //client to request nopCommerce official site
        //    services.AddHttpClient<NopHttpClient>().WithProxy();

        //    //client to request reCAPTCHA service
        //    services.AddHttpClient<CaptchaHttpClient>().WithProxy();
        //}
    }
}

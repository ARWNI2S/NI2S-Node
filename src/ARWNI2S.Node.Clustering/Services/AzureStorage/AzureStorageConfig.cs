﻿namespace ARWNI2S.Clustering.Services.AzureStorage
{
    /// <summary>
    /// Represents Azure Blob storage configuration parameters
    /// </summary>
    public partial class AzureBlobConfig : IConfig
    {
        /// <summary>
        /// Gets or sets connection string for Azure Blob storage
        /// </summary>
        [Secret]
        public string ConnectionString { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets container name for Azure Blob storage
        /// </summary>
        public string ContainerName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets end point for Azure Blob storage
        /// </summary>
        public string EndPoint { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets whether or the Container Name is appended to the AzureBlobStorageEndPoint when constructing the url
        /// </summary>
        public bool AppendContainerName { get; private set; } = true;

        /// <summary>
        /// Gets or sets whether to store Data Protection Keys in Azure Blob Storage
        /// </summary>
        public bool StoreDataProtectionKeys { get; private set; } = false;

        /// <summary>
        /// Gets or sets the Azure container name for storing Data Prtection Keys (this container should be separate from the container used for media and should be Private)
        /// </summary>
        public string DataProtectionKeysContainerName { get; private set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Azure key vault ID used to encrypt the Data Protection Keys. (this is optional)
        /// </summary>
        public string DataProtectionKeysVaultId { get; private set; } = string.Empty;

        /// <summary>
        /// Gets a value indicating whether we should use Azure Blob storage
        /// </summary>
        [JsonIgnore]
        public bool Enabled => !string.IsNullOrEmpty(ConnectionString);

        /// <summary>
        /// Whether to use an Azure Key Vault to encrypt the Data Protection Keys
        /// </summary>
        [JsonIgnore]
        public bool DataProtectionKeysEncryptWithVault => !string.IsNullOrEmpty(DataProtectionKeysVaultId);

        /// <inheritdoc/>
        public int GetOrder() => 2;
    }

    ///// <summary>
    ///// Represents Azure Tables storage configuration parameters
    ///// </summary>
    //public partial class AzureTablesConfig : IConfig
    //{
    //    /// <summary>
    //    /// Gets or sets connection string for Azure Tables storage
    //    /// </summary>
    //    [Secret]
    //    public string ConnectionString { get; private set; } = string.Empty;

    //    /// <summary>
    //    /// Gets or sets table name for Azure Tables storage
    //    /// </summary>
    //    public string DefaultTableName { get; private set; } = string.Empty;

    //    /// <summary>
    //    /// Gets or sets the Azure Tables service endpoint
    //    /// </summary>
    //    public string EndPoint { get; private set; } = string.Empty;

    //    /// <summary>
    //    /// Gets a value indicating whether we should use Azure Tables storage
    //    /// </summary>
    //    [JsonIgnore]
    //    public bool Enabled => !string.IsNullOrEmpty(ConnectionString);

    //    /// <summary>
    //    /// Gets or sets whether the Table Name is appended to the AzureTablesStorageEndPoint when constructing the url
    //    /// </summary>
    //    public bool SingleTableStorage { get; private set; } = true;
    //}
}
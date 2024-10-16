namespace ARWNI2S.Node.Services.Configuration
{
    public partial interface IAzureKeyVaultService
    {
        Task SetSecretAsync(string secretName, string secretValue);
        Task<string> GetSecretAsync(string secretName);
        Task DeleteSecretAsync(string secretName, bool purgeAfterDelete = false);
    }
}

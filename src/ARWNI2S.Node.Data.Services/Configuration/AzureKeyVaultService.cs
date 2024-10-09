using ARWNI2S.Node.Core.Infrastructure;
using Azure.Identity;
using Azure.Security.KeyVault.Secrets;

namespace ARWNI2S.Node.Data.Services.Configuration
{
    public partial class AzureKeyVaultService : IAzureKeyVaultService
    {
        //TODO: MOVE TO SETTINGS
        private readonly string _keyVaultName = "ni2s-keys";

        public AzureKeyVaultService()
        {

        }

        #region Utilities

        protected virtual SecretClient GetSecretClient()
        {
            //TODO: MOVE TO SETTINGS? <-- really no needed
            var kvUri = $"https://{_keyVaultName}.vault.azure.net";

            Singleton<SecretClient>.Instance ??= new SecretClient(new Uri(kvUri), new DefaultAzureCredential());

            return Singleton<SecretClient>.Instance;
        }

        #endregion

        #region Methods

        public virtual async Task SetSecretAsync(string secretName, string secretValue)
        {
            await GetSecretClient().SetSecretAsync(secretName, secretValue);
            //TODO: ERROR CONTROL (not connecting, can't write, etc...)
        }

        public virtual async Task<string> GetSecretAsync(string secretName)
        {
            var secret = await GetSecretClient().GetSecretAsync(secretName);
            //TODO: ERROR CONTROL (not connecting, can't write, etc...)
            return secret.Value.Value;
        }
        public virtual async Task DeleteSecretAsync(string secretName, bool purgeAfterDelete = false)
        {
            var client = GetSecretClient();

            DeleteSecretOperation operation = await client.StartDeleteSecretAsync(secretName);
            //TODO: ERROR CONTROL (not connecting, can't write, etc...)

            if (purgeAfterDelete)
            {
                // You only need to wait for completion if you want to purge or recover the secret.
                await operation.WaitForCompletionAsync();
                //TODO: ERROR CONTROL (not connecting, can't write, etc...)

                await client.PurgeDeletedSecretAsync(secretName);
                //TODO: ERROR CONTROL (not connecting, can't write, etc...)
            }
        }

        #endregion
    }
}
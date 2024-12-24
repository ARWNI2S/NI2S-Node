using Azure.Identity;
using Azure.Security.KeyVault.Secrets;
using Microsoft.Extensions.Configuration.UserSecrets;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System.Reflection;

namespace ARWNI2S.Engine.Security.Secrets
{
    internal class SecretValueProvider : IValueProvider
    {
        private readonly bool _isDevelopment;
        PropertyInfo _propertyInfo;

        public SecretValueProvider(PropertyInfo propertyInfo)
        {
            // Determina si estás en un entorno de desarrollo
            _isDevelopment = Environment.GetEnvironmentVariable("NI2S_ENVIRONMENT") == "Development";
            _propertyInfo = propertyInfo;
        }

        public object GetValue(object target)
        {
            if (_isDevelopment)
            {
                //En un entorno de desarrollo, lee el valor de User Secrets
                var userSecretsId = Assembly.GetEntryAssembly().GetCustomAttribute<UserSecretsIdAttribute>().UserSecretsId;
                var secretsJsonPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "Microsoft", "UserSecrets", userSecretsId, "secrets.json");
                var secretsJson = File.ReadAllText(secretsJsonPath);
                var secrets = JsonConvert.DeserializeObject<Dictionary<string, string>>(secretsJson);

                return secrets[_propertyInfo.Name];
            }
            else
            {
                // En un entorno de producción, lee el valor de Azure Key Vault
                var keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
                var keyVaultUri = $"https://{keyVaultName}.vault.azure.net";
                var credential = new DefaultAzureCredential();
                var client = new SecretClient(new Uri(keyVaultUri), credential);

                return client.GetSecret(_propertyInfo.Name).Value.Value;
            }
        }

        public void SetValue(object target, object value)
        {
            // Aquí puedes guardar el valor de la propiedad en el origen correcto (Azure Key Vault o User Secrets locales)
            if (!_isDevelopment)
            {
                // En un entorno de producción, guarda el valor en Azure Key Vault
                var keyVaultName = Environment.GetEnvironmentVariable("KEY_VAULT_NAME");
                var keyVaultUri = $"https://{keyVaultName}.vault.azure.net";
                var credential = new DefaultAzureCredential();
                var client = new SecretClient(new Uri(keyVaultUri), credential);

                client.SetSecret(_propertyInfo.Name, value.ToString());
            }
        }
    }
}

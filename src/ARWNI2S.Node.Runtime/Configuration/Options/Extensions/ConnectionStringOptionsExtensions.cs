using StackExchange.Redis;

namespace ARWNI2S.Node.Configuration.Options.Extensions
{
    public static class ConnectionStringOptionsExtensions
    {
        public static ConfigurationOptions ParseConnectionString(this ConfigurationOptions configOptions, string connectionString)
        {
            // Dividir la cadena de conexión en pares clave-valor
            var keyValuePairs = connectionString.Split(';')
                                                .Select(part => part.Split('='))
                                                .Where(part => part.Length == 2)
                                                .ToDictionary(sp => sp[0].Trim(), sp => sp[1].Trim());

            foreach (var kvp in keyValuePairs)
            {
                switch (kvp.Key)
                {
                    case "Data Source":
                    case "Server":
                        // Configurar los endpoints para Redis
                        configOptions.EndPoints.Add(kvp.Value);
                        break;
                    case "Connect Timeout":
                        // Configurar el tiempo de espera de conexión
                        if (int.TryParse(kvp.Value, out int timeout))
                            configOptions.ConnectTimeout = timeout * 1000; // En milisegundos
                        break;
                    case "User":
                        // Configurar el usuario de autenticación
                        configOptions.User = kvp.Value;
                        break;
                    case "Password":
                        // Configurar la contraseña de autenticación
                        configOptions.Password = kvp.Value;
                        break;
                    case "Encrypt":
                        // Configurar SSL para Redis si es necesario
                        configOptions.Ssl = bool.Parse(kvp.Value);
                        break;
                    case "Trust Server Certificate":
                        // Configurar la validación del certificado
                        configOptions.CheckCertificateRevocation = bool.Parse(kvp.Value);
                        break;
                    default:
                        // Ignorar claves no compatibles
                        break;
                }
            }

            return configOptions;
        }
    }
}

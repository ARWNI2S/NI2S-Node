using ARWNI2S.Infrastructure;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace ARWNI2S.Node.Runtime.Infrastructure
{
    public class OrleansStartup : INI2SStartup
    {
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            services.AddOrleans(siloBuilder =>
            {
                // Configurar el silo Orleans
                siloBuilder.UseLocalhostClustering();

                siloBuilder.AddMemoryGrainStorage("Default"); // Almacenamiento en memoria para grains
            });
        }

        public void Configure(IHost application)
        {
            // Orleans normalmente no necesita configuraciones adicionales aquí
        }

        public int Order => 150;  // Puedes ajustar el orden según sea necesario
    }
}

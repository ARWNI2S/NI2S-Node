using ARWNI2S.Data.Migrations;
using ARWNI2S.Engine.Extensibility;
using ARWNI2S.Engine.Hosting;
using ARWNI2S.Engine.Infrastructure;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Data
{
    public class NI2SDataModule : IEngineModule
    {
        /// <summary>
        /// Add and configure any of the engine services
        /// </summary>
        /// <param name="services">Collection of service descriptors</param>
        /// <param name="configuration">Configuration of the application</param>
        public void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var typeFinder = Singleton<ITypeFinder>.Instance;
            var mAssemblies = typeFinder.FindClassesOfType<MigrationBase>()
                .Select(t => t.Assembly)
                .Where(assembly => !assembly.FullName.Contains("FluentMigrator.Runner"))
                .Distinct()
                .ToArray();

            services
                // add common FluentMigrator services
                .AddFluentMigratorCore()
                .AddScoped<IProcessorAccessor, NI2SProcessorAccessor>()
                // set accessor for the connection string
                .AddScoped<IConnectionStringAccessor>(x => DataSettingsManager.LoadSettings())
                .AddSingleton<IMigrationManager, MigrationManager>()
                .AddSingleton<IConventionSet, NI2SConventionSet>()
                //.AddTransient<IMappingEntityAccessor>(x => x.GetRequiredService<IDataProviderManager>().DataProvider)
                .ConfigureRunner(rb =>
                    rb.WithVersionTable(new MigrationVersionInfo()).AddSqlServer().AddMySql5().AddPostgres()
                        // define the assembly containing the migrations
                        .ScanIn(mAssemblies).For.Migrations());

            services.AddTransient(p => new Lazy<IVersionLoader>(p.GetRequiredService<IVersionLoader>()));

            //data layer
            services.AddTransient<IDataProviderManager, DataProviderManager>();
            services.AddTransient(serviceProvider =>
                serviceProvider.GetRequiredService<IDataProviderManager>().DataProvider);

            //repositories	
            services.AddScoped(typeof(IRepository<>), typeof(EntityRepository<>));

            if (!DataSettingsManager.IsDatabaseInstalled())
                return;

            using var scope = services.BuildServiceProvider().CreateScope();
            var runner = scope.ServiceProvider.GetRequiredService<IMigrationManager>();
            foreach (var assembly in mAssemblies)
                runner.ApplyUpMigrations(assembly, MigrationProcessType.NoDependencies);
        }

        /// <summary>
        /// Configure the using of added components
        /// </summary>
        /// <param name="engine">Builder for configuring a node's NI2S engine</param>
        public void ConfigureEngine(IEngineBuilder engine)
        {

        }

        ///// <summary>
        ///// Gets order of this startup configuration implementation
        ///// </summary>
        //public int Order => InitStage.DbInit;
    }
}

﻿using ARWNI2S.Engine.Data.Migrations;
using ARWNI2S.Engine.Extensibility;
using FluentMigrator;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Conventions;
using FluentMigrator.Runner.Initialization;
using FluentMigrator.Runner.Processors;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace ARWNI2S.Engine.Data
{
    public class NI2SDataModule : EngineModule
    {
        public override int Order => 0;

        public override void ConfigureServices(IServiceCollection services, IConfiguration configuration)
        {
            var mAssemblies = TypeFinder.FindClassesOfType<MigrationBase>()
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
    }
}
﻿using FluentMigrator;
using FluentMigrator.Exceptions;
using FluentMigrator.Runner.Processors;

namespace ARWNI2S.Node.Data.Migrations
{
    /// <summary>
    /// An <see cref="IProcessorAccessor"/> implementation that selects one generator by data settings
    /// </summary>
    public class NI2SProcessorAccessor : IProcessorAccessor
    {
        #region Ctor

        public NI2SProcessorAccessor(IEnumerable<IMigrationProcessor> processors)
        {
            ConfigureProcessor(processors.ToList());
        }

        #endregion

        #region Utils

        /// <summary>
        /// Configure processor
        /// </summary>
        /// <param name="processors">Collection of migration processors</param>
        protected virtual void ConfigureProcessor(IList<IMigrationProcessor> processors)
        {
            var dataSettings = DataSettingsManager.LoadSettings();

            if (processors.Count == 0)
                throw new ProcessorFactoryNotFoundException("No migration processor registered.");

            if (dataSettings is null)
                Processor = processors.FirstOrDefault();
            else
                Processor = dataSettings.DataProvider switch
                {
                    DataProviderType.SqlServer => FindGenerator(processors, "SqlServer"),
                    DataProviderType.MySql => FindGenerator(processors, "MySQL"),
                    DataProviderType.PostgreSQL => FindGenerator(processors, "Postgres"),
                    _ => throw new ProcessorFactoryNotFoundException(
                        $@"A migration generator for Data provider type {dataSettings.DataProvider} couldn't be found.")
                };
        }

        /// <summary>
        /// Gets single processor by DatabaseType or DatabaseTypeAlias
        /// </summary>
        /// <param name="processors">Collection of migration processors</param>
        /// <param name="processorsId">DatabaseType or DatabaseTypeAlias</param>
        /// <returns></returns>
        protected static IMigrationProcessor FindGenerator(IList<IMigrationProcessor> processors,
            string processorsId)
        {
            if (processors.FirstOrDefault(p =>
                    p.DatabaseType.Equals(processorsId, StringComparison.OrdinalIgnoreCase) ||
                    p.DatabaseTypeAliases.Any(a => a.Equals(processorsId, StringComparison.OrdinalIgnoreCase))) is
                IMigrationProcessor processor)
                return processor;

            var generatorNames = string.Join(", ",
                processors.Select(p => p.DatabaseType).Union(processors.SelectMany(p => p.DatabaseTypeAliases)));

            throw new ProcessorFactoryNotFoundException(
                $@"A migration generator with the ID {processorsId} couldn't be found. Available generators are: {generatorNames}");
        }

        #endregion

        #region  Properties

        public IMigrationProcessor Processor { get; protected set; }

        #endregion
    }
}
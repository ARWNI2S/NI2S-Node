using ARWNI2S.Data.Entities;
using ARWNI2S.Data.Extensions;
using ARWNI2S.Data.Migrations;
using ARWNI2S.Infrastructure;
using FluentMigrator.Builders.Create.Table;
using FluentMigrator.Expressions;
using LinqToDB.Mapping;
using System.Collections.Concurrent;

namespace ARWNI2S.Data.Mapping
{
    /// <summary>
    /// Provides an access to entity mapping information
    /// </summary>
    public static class NI2SDataMappingSchema
    {
        #region Fields

        private static ConcurrentDictionary<Type, NI2SDataEntityDescriptor> EntityDescriptors { get; } = new();

        #endregion

        /// <summary>
        /// Returns mapped entity descriptor
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <returns>Mapped entity descriptor</returns>
        public static NI2SDataEntityDescriptor GetEntityDescriptor(Type entityType)
        {
            if (!typeof(DataEntity).IsAssignableFrom(entityType))
                return null;

            return EntityDescriptors.GetOrAdd(entityType, t =>
            {
                var tableName = NameCompatibilityManager.GetTableName(t);
                var expression = new CreateTableExpression { TableName = tableName };
                var builder = new CreateTableExpressionBuilder(expression, new NullMigrationContext());
                builder.RetrieveTableExpressions(t);

                return new NI2SDataEntityDescriptor
                {
                    EntityName = tableName,
                    SchemaName = builder.Expression.SchemaName,
                    Fields = builder.Expression.Columns.Select(column => new NI2SDataEntityFieldDescriptor
                    {
                        Name = column.Name,
                        IsPrimaryKey = column.IsPrimaryKey,
                        IsNullable = column.IsNullable,
                        Size = column.Size,
                        Precision = column.Precision,
                        IsIdentity = column.IsIdentity,
                        Type = column.Type ?? System.Data.DbType.String
                    }).ToList()
                };
            });
        }

        /// <summary>
        /// Get or create mapping schema with specified configuration name
        /// </summary>
        public static MappingSchema GetMappingSchema(string configurationName)
        {

            if (Singleton<MappingSchema>.Instance is null)
            {
                Singleton<MappingSchema>.Instance = new MappingSchema(configurationName);
                Singleton<MappingSchema>.Instance.AddMetadataReader(new FluentMigratorMetadataReader());
            }

            return Singleton<MappingSchema>.Instance;
        }
    }
}
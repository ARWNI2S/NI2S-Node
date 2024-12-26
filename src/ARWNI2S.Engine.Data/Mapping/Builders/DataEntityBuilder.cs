using ARWNI2S.Engine.Data.Entities;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Engine.Data.Mapping.Builders
{
    /// <summary>
    /// Represents base data entity builder
    /// </summary>
    /// <typeparam name="TEntity">Data entity type</typeparam>
    /// <remarks>
    /// Entity type <typeparamref name="TEntity"/> is needed to determine the right entity builder for a specific entity type
    /// </remarks>
    public abstract partial class DataEntityBuilder<TEntity> : IDataEntityBuilder where TEntity : DataEntity
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public abstract void MapEntity(CreateTableExpressionBuilder table);
    }
}
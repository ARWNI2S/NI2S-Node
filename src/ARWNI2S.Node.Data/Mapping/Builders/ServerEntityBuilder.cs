using ARWNI2S.Node.Data.Entities;
using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders
{
    /// <summary>
    /// Represents base entity builder
    /// </summary>
    /// <typeparam name="TEntity">Entity type</typeparam>
    /// <remarks>
    /// Entity type <typeparamref name="TEntity"/> is needed to determine the right entity builder for a specific entity type
    /// </remarks>
    public abstract partial class ServerEntityBuilder<TEntity> : IEntityBuilder where TEntity : BaseDataEntity
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        public abstract void MapEntity(CreateTableExpressionBuilder table);
    }
}
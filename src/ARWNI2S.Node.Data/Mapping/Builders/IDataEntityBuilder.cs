using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Node.Data.Mapping.Builders
{
    /// <summary>
    /// Represents database entity builder
    /// </summary>
    public interface IDataEntityBuilder
    {
        /// <summary>
        /// Apply entity configuration
        /// </summary>
        /// <param name="table">Create table expression builder</param>
        void MapEntity(CreateTableExpressionBuilder table);
    }
}

// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using FluentMigrator.Builders.Create.Table;

namespace ARWNI2S.Engine.Data.Mapping.Builders
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

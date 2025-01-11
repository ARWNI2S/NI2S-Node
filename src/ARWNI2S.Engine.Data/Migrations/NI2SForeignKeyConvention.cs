// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using FluentMigrator.Expressions;
using FluentMigrator.Model;
using FluentMigrator.Runner.Conventions;

namespace ARWNI2S.Engine.Data.Migrations
{
    /// <summary>
    /// Convention for the default naming of a foreign key
    /// </summary>
    public class NI2SForeignKeyConvention : IForeignKeyConvention
    {
        #region Fields

        private readonly INiisDataProvider _dataProvider;

        #endregion

        #region Ctor

        public NI2SForeignKeyConvention(INiisDataProvider dataProvider)
        {
            _dataProvider = dataProvider;
        }

        #endregion

        #region Utils

        /// <summary>
        /// Gets the default name of a foreign key
        /// </summary>
        /// <param name="foreignKey">The foreign key definition</param>
        /// <returns>Name of a foreign key</returns>
        private string GetForeignKeyName(ForeignKeyDefinition foreignKey)
        {
            var foreignColumns = string.Join('_', foreignKey.ForeignColumns);
            var primaryColumns = string.Join('_', foreignKey.PrimaryColumns);

            var keyName = _dataProvider.CreateForeignKeyName(foreignKey.ForeignTable, foreignColumns, foreignKey.PrimaryTable, primaryColumns);

            return keyName;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Applies a convention to a FluentMigrator.Expressions.IForeignKeyExpression
        /// </summary>
        /// <param name="expression">The expression this convention should be applied to</param>
        /// <returns>The same or a new expression. The underlying type must stay the same</returns>
        public IForeignKeyExpression Apply(IForeignKeyExpression expression)
        {
            if (string.IsNullOrEmpty(expression.ForeignKey.Name))
                expression.ForeignKey.Name = GetForeignKeyName(expression.ForeignKey);

            return expression;
        }

        #endregion
    }
}
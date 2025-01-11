// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using FluentMigrator;
using FluentMigrator.Expressions;
using FluentMigrator.Infrastructure;

namespace ARWNI2S.Engine.Data.Migrations
{
    /// <summary>
    /// Represents the migration context with a null implementation of a processor that does not do any work
    /// </summary>
    public class NullMigrationContext : IMigrationContext
    {
        public IServiceProvider ServiceProvider { get; set; }

        public ICollection<IMigrationExpression> Expressions { get; set; } = [];

        public IQuerySchema QuerySchema { get; set; }
#pragma warning disable 612
        public IAssemblyCollection MigrationAssemblies { get; set; }
        public object ApplicationContext { get; set; }
#pragma warning restore 612
        public string Connection { get; set; }
    }
}
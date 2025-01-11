// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Data.Migrations
{
    /// <summary>
    /// Represents the type of migration process
    /// </summary>
    public enum MigrationProcessType
    {
        /// <summary>
        /// The type of migration process does not matter
        /// </summary>
        NoMatter,

        /// <summary>
        /// Installation
        /// </summary>
        Installation,

        /// <summary>
        /// Update
        /// </summary>
        Update,

        /// <summary>
        /// Apply migration right after the migration runner will become available
        /// </summary>
        ///<remarks>
        /// Avoid using dependency injection in migrations that are marked by this type of migration process,
        /// because many dependencies are not registered yet.
        ///</remarks>
        NoDependencies
    }
}
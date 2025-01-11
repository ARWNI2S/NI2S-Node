// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using LinqToDB.Mapping;

namespace ARWNI2S.Engine.Data.Mapping
{
    /// <summary>
    /// Represents interface to implement an accessor to mapped entities
    /// </summary>
    public interface IMappingEntityAccessor
    {
        /// <summary>
        /// Returns mapped entity descriptor
        /// </summary>
        /// <param name="entityType">Type of entity</param>
        /// <returns>Mapped entity descriptor</returns>
        NI2SDataEntityDescriptor GetEntityDescriptor(Type entityType);

        /// <summary>
        /// Get entity accessor mapping schema
        /// </summary>
        MappingSchema GetMappingSchema();
    }
}
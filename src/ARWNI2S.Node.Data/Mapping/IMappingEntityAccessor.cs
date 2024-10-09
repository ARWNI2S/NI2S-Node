using LinqToDB.Mapping;

namespace ARWNI2S.Node.Data.Mapping
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
        ServerEntityDescriptor GetEntityDescriptor(Type entityType);

        /// <summary>
        /// Get entity accessor mapping schema
        /// </summary>
        MappingSchema GetMappingSchema();
    }
}
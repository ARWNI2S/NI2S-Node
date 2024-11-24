using ARWNI2S.Node.Core;

namespace ARWNI2S.Node.Data.Notification
{
    /// <summary>
    /// A container for passing entities that have been deleted. This is not used for entities that are deleted logically via a bit column.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public partial class DataEntityDeleted<T> where T : IEntity
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="entity">Entity</param>
        public DataEntityDeleted(T entity)
        {
            Entity = entity;
        }

        /// <summary>
        /// Entity
        /// </summary>
        public T Entity { get; }
    }
}

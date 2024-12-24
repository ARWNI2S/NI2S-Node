using ARWNI2S.Data.Entities;
using ARWNI2S.Engine.Common.Data;

namespace ARWNI2S.Engine.Common
{
    /// <summary>
    /// Generic attribute service interface
    /// </summary>
    public partial interface IGenericAttributeService
    {
        /// <summary>
        /// Deletes an attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteAttributeAsync(GenericAttribute attribute);

        /// <summary>
        /// Deletes an attributes
        /// </summary>
        /// <param name="attributes">Attributes</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task DeleteAttributesAsync(IList<GenericAttribute> attributes);

        /// <summary>
        /// Inserts an attribute
        /// </summary>
        /// <param name="attribute">attribute</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task InsertAttributeAsync(GenericAttribute attribute);

        /// <summary>
        /// Updates the attribute
        /// </summary>
        /// <param name="attribute">Attribute</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task UpdateAttributeAsync(GenericAttribute attribute);

        /// <summary>
        /// Get attributes
        /// </summary>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="keyGroup">Key group</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the get attributes
        /// </returns>
        Task<IList<GenericAttribute>> GetAttributesForEntityAsync(int entityId, string keyGroup);

        /// <summary>
        /// Save attribute value
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="value">Value</param>
        /// <param name="serverId">Server identifier; pass 0 if this attribute will be available for all servers</param>
        /// <returns>A task that represents the asynchronous operation</returns>
        Task SaveAttributeAsync<TPropType>(DataEntity entity, string key, TPropType value, int serverId = 0);

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <param name="entity">Entity</param>
        /// <param name="key">Key</param>
        /// <param name="serverId">Load a value specific for a certain server; pass 0 to load a value shared for all servers</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the attribute
        /// </returns>
        Task<TPropType> GetAttributeAsync<TPropType>(DataEntity entity, string key, int serverId = 0, TPropType defaultValue = default);

        /// <summary>
        /// Get an attribute of an entity
        /// </summary>
        /// <typeparam name="TPropType">Property type</typeparam>
        /// <typeparam name="TEntity">Entity type</typeparam>
        /// <param name="entityId">Entity identifier</param>
        /// <param name="key">Key</param>
        /// <param name="serverId">Load a value specific for a certain server; pass 0 to load a value shared for all servers</param>
        /// <param name="defaultValue">Default value</param>
        /// <returns>
        /// A task that represents the asynchronous operation
        /// The task result contains the attribute
        /// </returns>
        Task<TPropType> GetAttributeAsync<TEntity, TPropType>(int entityId, string key, int serverId = 0, TPropType defaultValue = default)
            where TEntity : DataEntity;
    }
}
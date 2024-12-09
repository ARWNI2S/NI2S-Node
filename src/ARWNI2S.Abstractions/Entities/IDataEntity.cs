namespace ARWNI2S.Entities
{
    public interface IDataEntity : IEntity
    {
        /// <summary>
        /// Gets or sets the entity identifier
        /// </summary>
        new int Id { get; set; }

        object IEntity.Id => Id;
    }
}

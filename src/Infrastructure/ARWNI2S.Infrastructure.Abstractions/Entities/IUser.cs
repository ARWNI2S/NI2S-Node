namespace ARWNI2S.Infrastructure.Entities
{
    public interface IUser : IEntity
    {
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        new int Id { get; }

        /// <summary>
        /// Gets or sets the time zone id
        /// </summary>
        string TimeZoneId { get; set; }

    }
}

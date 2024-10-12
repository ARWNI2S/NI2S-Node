namespace ARWNI2S.Infrastructure.Entities
{
    public interface INI2SUser
    {
        /// <summary>
        /// Gets or sets the user id
        /// </summary>
        int Id { get; }

        /// <summary>
        /// Gets or sets the time zone id
        /// </summary>
        string TimeZoneId { get; set; }

    }
}

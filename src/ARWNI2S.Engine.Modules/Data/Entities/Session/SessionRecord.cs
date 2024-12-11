namespace ARWNI2S.Node.Core.Entities.Session
{
    [Flags]
    public enum ConnectionState
    {
        /// <summary>
        /// Sin conexion.
        /// </summary>
        Disconnected = 0,
        /// <summary>
        /// Editor session
        /// </summary>
        EditMode = 1,
        /// <summary>
        /// Actually connected
        /// </summary>
        Connected = 2,
        /// <summary>
        /// Actually playing
        /// </summary>
        Playing = 4,
        /// <summary>
        /// Session error
        /// </summary>
        Error = 99,
    }

    public partial class SessionRecord : DataEntity
    {
        public int UserId { get; set; }
        public string SessionId { get; set; }
        public DateTime? ExpiresOnUtc { get; set; }

        public DateTime CreatedOnUtc { get; set; }
        public DateTime? UpdatedOnUtc { get; set; }

        // Nuevo campo para el estado de la sesión
        public ConnectionState State { get; set; }

        // Referencias a entidades Orleans cuando el usuario está en estado "InGame"
        public Guid? PersonajeId { get; set; }
        public Guid? EscenaId { get; set; }
    }
}

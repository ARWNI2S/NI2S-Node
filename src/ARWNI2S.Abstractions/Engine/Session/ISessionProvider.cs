namespace ARWNI2S.Engine.Session
{
    /// <summary>
    /// Provides access to the <see cref="INiisSession"/> for the current process.
    /// </summary>
    public interface ISessionProvider
    {
        /// <summary>
        /// The <see cref="INiisSession"/> for the current process.
        /// </summary>
        INiisSession Session { get; set; }
    }
}

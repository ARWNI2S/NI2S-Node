namespace ARWNI2S.Engine.Session
{
    /// <summary>
    /// Storage for sessions that maintain user data while the user browses a web application.
    /// </summary>
    public interface ISessionStore
    {
        /// <summary>
        /// Create a new or resume an <see cref="INiisSession"/>.
        /// </summary>
        /// <param name="sessionKey">A unique key used to lookup the session.</param>
        /// <param name="idleTimeout">How long the session can be inactive (e.g. not accessed) before it will expire.</param>
        /// <param name="ioTimeout">
        /// The maximum amount of time <see cref="INiisSession.LoadAsync(CancellationToken)"/> and
        /// <see cref="INiisSession.CommitAsync(CancellationToken)"/> are allowed take.
        /// </param>
        /// <param name="tryEstablishSession">
        /// A callback invoked during <see cref="INiisSession.Set(string, byte[])"/> to verify that modifying the session is currently valid.
        /// If the callback returns <see langword="false"/>, <see cref="INiisSession.Set(string, byte[])"/> should throw an <see cref="InvalidOperationException"/>.
        /// prior to sending the response.
        /// </param>
        /// <param name="isNewSessionKey"><see langword="true"/> if establishing a new session; <see langword="false"/> if resuming a session.</param>
        /// <returns>The <see cref="INiisSession"/> that was created or resumed.</returns>
        INiisSession Create(string sessionKey, TimeSpan idleTimeout, TimeSpan ioTimeout, Func<bool> tryEstablishSession, bool isNewSessionKey);
    }
}

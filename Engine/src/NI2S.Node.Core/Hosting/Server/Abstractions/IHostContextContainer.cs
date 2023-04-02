namespace NI2S.Node.Hosting.Server.Abstractions
{
    /// <summary>
    /// When implemented by a Server allows an <see cref="IDummyApplication{TContext}"/> to pool and reuse
    /// its <typeparamref name="TContext"/> between requests.
    /// </summary>
    /// <typeparam name="TContext">The <see cref="IDummyApplication{TContext}"/> Host context</typeparam>
    public interface IHostContextContainer<TContext> where TContext : notnull
    {
        /// <summary>
        /// Represents the <typeparamref name="TContext"/>  of the host.
        /// </summary>
        TContext HostContext { get; set; }
    }
}

﻿namespace ARWNI2S.Node.Hosting
{
    /// <summary>
    /// Allows consumers to perform cleanup during a graceful shutdown.
    /// <para>
    ///  This type is obsolete and will be removed in a future version.
    ///  The recommended alternative is Microsoft.Extensions.Hosting.IHostApplicationLifetime.
    /// </para>
    /// </summary>
    [Obsolete("This type is obsolete and will be removed in a future version. The recommended alternative is Microsoft.Extensions.Hosting.IHostApplicationLifetime.", error: false)]
    public interface IEngineLifetime
    {
        /// <summary>
        /// Triggered when the application host has fully started and is about to wait
        /// for a graceful shutdown.
        /// </summary>
        CancellationToken ApplicationStarted { get; }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// Requests may still be in flight. Shutdown will block until this event completes.
        /// </summary>
        CancellationToken ApplicationStopping { get; }

        /// <summary>
        /// Triggered when the application host is performing a graceful shutdown.
        /// All requests should be complete at this point. Shutdown will block
        /// until this event completes.
        /// </summary>
        CancellationToken ApplicationStopped { get; }

        /// <summary>
        /// Requests termination of the current application.
        /// </summary>
        void StopApplication();
    }
}

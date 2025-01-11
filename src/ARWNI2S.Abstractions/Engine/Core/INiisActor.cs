// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Core
{
    public interface INiisActor : INiisObject
    {
        IEnumerable<IActorComponent> Components { get; }

        Task OnActivateAsync(CancellationToken cancellationToken);
        Task OnDeactivateAsync(DeactivationReason reason, CancellationToken cancellationToken);
    }

    /// <summary>
    /// Represents a reason for initiating grain deactivation.
    /// </summary>
    public readonly struct DeactivationReason
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeactivationReason"/> struct.
        /// </summary>
        /// <param name="code">
        /// The code identifying the deactivation reason.
        /// </param>
        /// <param name="text">
        /// A descriptive reason for the deactivation.
        /// </param>
        public DeactivationReason(DeactivationReasonCode code, string text)
        {
            ReasonCode = code;
            Description = text;
            Exception = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeactivationReason"/> struct.
        /// </summary>
        /// <param name="code">
        /// The code identifying the deactivation reason.
        /// </param>
        /// <param name="exception">
        /// The exception which resulted in deactivation.
        /// </param>
        /// <param name="text">
        /// A descriptive reason for the deactivation.
        /// </param>
        public DeactivationReason(DeactivationReasonCode code, Exception exception, string text)
        {
            ReasonCode = code;
            Description = text;
            Exception = exception;
        }

        /// <summary>
        /// Gets the descriptive reason for the deactivation.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets the reason for deactivation.
        /// </summary>
        public DeactivationReasonCode ReasonCode { get; }

        /// <summary>
        /// Gets the exception which resulted in deactivation.
        /// </summary>
        public Exception Exception { get; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Exception is not null)
            {
                return $"{ReasonCode}: {Description}. Exception: {Exception}";
            }

            return $"{ReasonCode}: {Description}";
        }
    }

    /// <summary>
    /// An informational reason code for deactivation.
    /// </summary>
    public enum DeactivationReasonCode : byte
    {
        /// <summary>
        /// No reason provided.
        /// </summary>
        None,

        /// <summary>
        /// The process is currently shutting down.
        /// </summary>
        ShuttingDown,

        /// <summary>
        /// Activation of the actor failed.
        /// </summary>
        ActivationFailed,

        /// <summary>
        /// This activation is affected by an internal failure in the distributed simulation directory.
        /// </summary>
        /// <remarks>
        /// This could be caused by the failure of a process hosting this activation's actor directory partition, for example.
        /// </remarks>
        DirectoryFailure,

        /// <summary>
        /// This activation is idle.
        /// </summary>
        ActivationIdle,

        /// <summary>
        /// This activation is unresponsive.
        /// </summary>
        ActivationUnresponsive,

        /// <summary>
        /// Another instance of this actor has been activated.
        /// </summary>
        DuplicateActivation,

        /// <summary>
        /// This activation cannot be handled by the locally running process.
        /// </summary>
        Incompatible,

        /// <summary>
        /// An engine error occurred.
        /// </summary>
        EngineError,

        /// <summary>
        /// The engine requested that this activation deactivate.
        /// </summary>
        EngineRequested,

        /// <summary>
        /// This activation is migrating to a new location.
        /// </summary>
        Migrating,
    }

    internal static class DeactivationReasonCodeExtensions
    {
        public static bool IsTransientError(this DeactivationReasonCode reasonCode)
        {
            return reasonCode is DeactivationReasonCode.DirectoryFailure;
        }
    }

}
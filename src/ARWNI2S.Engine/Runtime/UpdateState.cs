namespace ARWNI2S.Engine.Runtime
{
    /// <summary>
    /// Update function states
    /// </summary>
    public enum UpdateState
    {
        /// <summary>
        /// This update will not fire
        /// </summary>
        Disabled,
        /// <summary>
        /// This update will fire as usual
        /// </summary>
        Enabled,
        /// <summary>
        /// This update has a timed frequency and already has updated.
        /// </summary>
        Cooldown,
        /// <summary>
        /// This update is delayed and will be executed as soon as possible.
        /// </summary>
        Dirty
    }

}

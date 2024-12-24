namespace ARWNI2S.Engine.Runtime
{
    /// <summary>
    /// Internal Data structure that contains members only required for a registered update function
    /// </summary>
    public class RegistryInfo
    {
        public RegistryInfo() { }

        /// <summary>
        /// Whether the update function is registered.
        /// </summary>
        public bool Registered = true;
        /// <summary>
        /// Cache whether this function was rescheduled as an interval function during StartParallel
        /// </summary>
        public bool WasInterval = true;
        /// <summary>
        /// Internal data that indicates the update group we actually started in (it may have been delayed due to prerequisites)
        /// </summary>
        public UpdateGroup ActualUpdateGroup;
        /// <summary>
        /// Internal data that indicates the update group we actually started in (it may have been delayed due to prerequisites)
        /// </summary>
        public UpdateGroup ActualEndUpdateGroup;

        /// <summary>
        /// Internal data to track if we have started visiting this update function yet this frame
        /// </summary>
        public int FrameUpdateCount;
        /// <summary>
        /// Internal data to track if we have finished visiting this update function yet this frame
        /// </summary>
        public int QueuedFrameCounter;

        public Task Task;

        public UpdateFunction Next;

        /// <summary>
        /// If UpdateFrequency is greater than 0 and update state is CoolingDown, this is the time, 
        /// relative to the element ahead of it in the cooling down list, remaining until the next time this function will update
        /// </summary>
        public double RelativeCooldown;

        /// <summary>
        /// The last world game time at which we were updateed. Game time used is dependent on bUpdateEvenWhenPaused
        /// Valid only if we've been updateed at least once since having a update interval; otherwise set to -1.f
        /// </summary>
        public float LastUpdateGameTimeSeconds;

        /// <summary>
        /// Back pointer to the <see cref="Runtime.FrameRoot"/> containing this update function if it is registered
        /// </summary>
        public FrameRoot FrameRoot;

        /// <summary>
        /// Back pointer to the <see cref="Runtime.UpdateRoot"/> containing this update function if it is registered
        /// </summary>
        public UpdateRoot UpdateRoot;
    }
}

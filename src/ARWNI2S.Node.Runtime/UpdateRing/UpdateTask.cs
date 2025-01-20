using ARWNI2S.Engine.Object;
using ARWNI2S.Runtime.Simulation;

namespace ARWNI2S.Runtime.UpdateRing
{
    public enum LevelUpdate : byte
    {
    }

    public enum UpdateGroup : byte
    {
    }
    
    /// <summary>
    /// Abstract Base class for all update functions.
    /// </summary>
    internal abstract class UpdateTask
    {
        /// <summary>
        /// Defines the minimum update group for this update function. These groups determine the relative order of when objects update during a frame update.
        /// Given prerequisites, the update may be delayed.
        /// <see cref="UpdateGroup"/>
        /// <see cref="UpdateTask.AddPrerequisite"/>
        /// </summary>
        public UpdateGroup UpdateGroup { get; set; }

        /// <summary>
        /// Defines the update group that this update function must finish in. These groups determine the relative order of when objects update during a frame update.
        /// <see cref="UpdateGroup"/>
        /// </summary>
        public UpdateGroup EndUpdateGroup { get; set; }

        /// <summary>
        /// If false, this update function will never be registered and will never update. Only settable in defaults.
        /// </summary>
        public bool CanUpdate { get; set; } = true;

        /// <summary>
        /// If false, this update function will never be registered and will never update. Only settable in defaults.
        /// </summary>
        public bool StartEnabled { get; set; } = true;

        /// <summary>
        /// This update must be executed distribued in every node it exists.
        /// </summary>
        public bool DistributedUpdate { get; set; } = true;

        /// <summary>
        /// Run this update first within the update group, presumably to start async tasks that must be completed with this update group, hiding the latency.
        /// </summary>
        public bool HighPriority { get; set; } = true;

        /// <summary>
        /// If false, this update will run on the game thread, otherwise it will run on any thread in parallel with the game thread and in parallel with other "async updates".
        /// </summary>
        public bool RunOnAnyThread { get; set; } = true;

        protected internal enum UpdateState : byte
        {
            Disabled,
            Enabled,
            CoolingDown
        };

        /// <summary>
        /// If Disabled, this update will not fire
        /// If CoolingDown, this update has an interval frequency that is being adhered to currently
        /// </summary>
        /// <remarks><b>CAUTION: Do not set this directly</b></remarks>
        protected internal UpdateState _updateState;

        /// <summary>
        /// The frequency in seconds at which this update function will be executed.  If less than or equal to 0 then it will update every frame.
        /// </summary>
        public float UpdateInterval;

        /// <summary>
        /// Prerequisites for this update function
        /// </summary>
        protected internal ICollection<UpdatePrerequisite> _prerequisites;

        /// <summary>
        /// Internal Data structure that contains members only required for a registered update function
        /// </summary>
        protected internal struct InternalData
        {
            public InternalData() { }

            /// <summary>
            /// Whether the update function is registered.
            /// </summary>
            public bool Registered { get; set; } = true;

            /// <summary>
            /// Cache whether this function was rescheduled as an interval function during StartParallel
            /// </summary>
            public bool WasInterval { get; set; } = true;

            /// <summary>
            /// Internal data that indicates the update group we actually started in (it may have been delayed due to prerequisites)
            /// </summary>
            public UpdateGroup StartUpdateGroup;

            /// <summary>
            /// Internal data that indicates the update group we actually started in (it may have been delayed due to prerequisites)
            /// </summary>
            public UpdateGroup EndUpdateGroup;

            /// <summary>
            /// Internal data to track if we have started visiting this update function yet this frame
            /// </summary>
            public int UpdateVisitedFrameCounter;

            /// <summary>
            /// Internal data to track if we have finished visiting this update function yet this frame
            /// </summary>
            public int UpdateQueuedFrameCounter;

            /// <summary>
            /// Pointer to the task, only used during setup. This is often stale.
            /// </summary>
            public Task TaskPointer;
            //public BaseGraphTask TaskPointer;

            /// <summary>
            /// The next function in the cooling down list for updates with an interval
            /// </summary>
            public UpdateTask Next;

            /// <summary> 
            /// If UpdateFrequency is greater than 0 and update state is CoolingDown, this is the time, 
            /// relative to the element ahead of it in the cooling down list, remaining until the next time this function will update 
            /// </summary>
            public float RelativeUpdateCooldown;

            /// <summary> 
            /// The last world game time at which we were updateed. Game time used is dependent on bUpdateEvenWhenPaused
            /// Valid only if we've been updateed at least once since having a update interval; otherwise set to -1.f
            /// </summary>
            public float LastUpdateGameTimeSeconds;

            /// <summary>
            /// Back pointer to the FUpdateTaskLevel containing this update function if it is registered
            /// </summary>
            public UpdateTaskLevel UpdateTaskLevel;
        }

        /// <summary>
        /// Lazily allocated struct that contains the necessary data for a update function that is registered.
        /// </summary>
        protected internal Lazy<InternalData> _internalData;

        /// <summary>
        /// Default ructor, intitalizes to reasonable defaults
        /// </summary>
        public UpdateTask() { }

        /// <summary>
        /// Destructor, unregisters the update function
        /// </summary>
        ~UpdateTask() { }

        /// <summary> 
        /// Adds the update function to the primary list of update functions. 
        /// @param Level - level to place this update function in
        /// </summary>
        public void RegisterUpdateFunction(Scene scene) { }
        /// <summary>
        /// Removes the update function from the primary list of update functions.
        /// </summary>
        public void UnRegisterUpdateFunction() { }
        /// <summary>
        /// See if the update function is currently registered
        /// </summary>
        public bool IsRegistered => (_internalData.IsValueCreated && _internalData.Value.Registered);

        /// <summary>
        /// Enables or disables this update function.
        /// </summary>
        public void SetEnable(bool enabled) { _updateState = enabled ? UpdateState.Enabled : UpdateState.Disabled; }
        /// <summary>
        /// Returns whether the update function is currently enabled
        /// </summary>
        public bool IsEnabled => _updateState != UpdateState.Disabled;

        /// <summary>
        /// Returns whether it is valid to access this update function's completion handle
        /// </summary>
        public bool CompletionHandleValid => (_internalData.IsValueCreated && _internalData.Value.TaskPointer != null);

        /// <summary>
        /// Update update interval in the system and overwrite the current cooldown if any.
        /// </summary>
        public void UpdateUpdateIntervalAndCoolDown(float newUpdateInterval) { }

        /// <summary>
        /// Gets the current completion handle of this update function, so it can be delayed until a later point when some additional
        /// tasks have been completed.  Only valid after TG_PreAsyncWork has started and then only until the UpdateFunction finishes
        /// execution
        /// </summary>
        public GraphEvent GetCompletionHandle() { return null; }

        /// <summary> 
        /// Gets the action update group that this function will be elligible to start in.
        /// Only valid after TG_PreAsyncWork has started through the end of the frame.
        /// </summary>
        public UpdateGroup ActualUpdateGroup => (_internalData.IsValueCreated ? _internalData.Value.StartUpdateGroup : UpdateGroup);

        /// <summary> 
        /// Gets the action update group that this function will be required to end in.
        /// Only valid after TG_PreAsyncWork has started through the end of the frame.
        /// </summary>
        public UpdateGroup ActualEndUpdateGroup => (_internalData.IsValueCreated ? _internalData.Value.EndUpdateGroup : EndUpdateGroup);

        /// <summary> 
        /// Adds a update function to the list of prerequisites...in other words, adds the requirement that TargetUpdateFunction is called before this update function is 
        ///  @param TargetObject - UObject containing this update function. Only used to verify that the other pointer is still usable
        /// @param TargetUpdateFunction - Actual update function to use as a prerequisite
        /// </summary>
        public void AddPrerequisite(NI2SObject targetObject, UpdateTask targetUpdateFunction) { }

        /// <summary> 
        /// Removes a prerequisite that was previously added.
        /// @param TargetObject - UObject containing this update function. Only used to verify that the other pointer is still usable
        /// @param TargetUpdateFunction - Actual update function to use as a prerequisite
        /// </summary>
        public void RemovePrerequisite(NI2SObject targetObject, UpdateTask targetUpdateFunction) { }

        /// <summary> 
        /// Sets this function to hipri and all prerequisites recursively
        /// @param bInHighPriority - priority to set
        /// </summary>
        public void SetPriorityIncludingPrerequisites(bool bInHighPriority) { }

        /// <summary>
        /// @return a reference to prerequisites for this update function.
        /// </summary>
        public ICollection<UpdatePrerequisite> GetPrerequisites() { return _prerequisites; }

        public float GetLastUpdateGameTime() { return (_internalData.IsValueCreated ? _internalData.Value.LastUpdateGameTimeSeconds : -1.0f); }

        /// <summary>
        /// Queues a update function for execution from the game thread
        /// @param UpdateContext - context to update in
        /// </summary>

        protected internal void QueueUpdateFunction(UpdateTaskSequencer tts, UpdateContext updateContext)
        {

        }

        /// <summary>
        /// Queues a update function for execution from the game thread
        ///  @param UpdateContext - context to update in
        /// @param StackForCycleDetection - Stack For Cycle Detection
        /// </summary>
        protected internal void QueueUpdateFunctionParallel(UpdateContext updateContext, IList<UpdateTask> stackForCycleDetection)
        {

        }

        /// <summary>
        /// Returns the delta time to use when updateing this function given the UpdateContext
        /// </summary>
        protected internal float CalculateDeltaTime(UpdateContext updateContext)
        {
            return -1.0f;
        }

        /// <summary> 
        /// Logs the prerequisites
        /// </summary>
        protected internal void ShowPrerequistes(int indent = 1) 
        {

        }

        /// <summary> 
        /// Abstract function actually execute the update. 
        /// @param DeltaTime - frame time to advance, in seconds
        /// @param UpdateType - kind of update for this frame
        /// @param CurrentThread - thread we are executing on, useful to pass along as new tasks are created
        /// @param MyCompletionGraphEvent - completion event for this task. Useful for holding the completetion of this task until certain child tasks are complete.
        /// </summary>
        protected internal abstract void ExecuteUpdate(float deltaTime, LevelUpdate updateType, Thread currentThread, GraphEvent MyCompletionGraphEvent);
        /// <summary>
        /// Abstract function to describe this update. Used to print messages about illegal cycles in the dependency graph
        /// </summary>
        protected internal abstract string DiagnosticMessage();
        /// <summary>
        /// /// Function to give a 'context' for this update, used for grouped active update reporting
        /// </summary>
        protected internal virtual Name DiagnosticContext(bool bDetailed)
        {
            return Name.None;
        }

        //friend class FUpdateTaskSequencer;
        //friend class FUpdateTaskManager;
        //friend class FUpdateTaskLevel;
        //friend class FUpdateFunctionTask;

        // It is unsafe to copy FUpdateFunctions and any subclasses of FUpdateFunction should specify the type trait WithCopy = false
        //UpdateFunction operator=(UpdateFunction) = delete;
    }
}
namespace ARWNI2S.Engine.Runtime
{
	/// <summary>
	/// Represents a simulation periodic update function
	/// </summary>
	public abstract partial class UpdateFunction
	{
		/// <summary>
		/// Defines the minimum update group for this update function.
		/// These groups determine the relative order of when objects update during a frame update.
		/// </summary>
		/// <remarks>Given <see cref="Requeriments"/>, the update may be delayed.</remarks>
		public UpdateGroup UpdateGroup { get; set; }

		/// <summary>
		/// Defines the update group that this update function must finish in.
		/// These groups determine the relative order of when objects update during a frame update.
		/// </summary>
		public UpdateGroup EndUpdateGroup { get; set; }

		/// <summary>
		/// If false the update function will never be registered, nor called.
		/// </summary>
		public bool CanUpdate { get; set; } = true;
		/// <summary>
		/// If true, this update function will start enabled, but can be disabled later on.
		/// </summary>
		public bool StartEnabled { get; set; } = true;
		/// <summary>
		/// If we allow this update to run on all layers
		/// </summary>
		public bool DistributedUpdate { get; set; } = true;
		/// <summary>
		/// Run this update first within the update group, presumably to start async tasks that must be completed with this update group,
		/// hiding the latency.
		/// </summary>
		public bool HighPriority { get; set; } = true;
		/// <summary>
		/// If false, this update will run on the main thread, otherwise it will run on any thread in parallel with the main thread and in
		/// parallel with other "async updates"
		/// </summary>
		public bool LongRunning { get; set; } = true;

		///// <summary>
		///// Level of detail in wich this update function is running.
		///// </summary>
		//public Resolution LOD { get; set; } = Resolution.Inactive;

		/// <summary>
		/// The state of the update function
		/// </summary>
		public UpdateState State { get; set; }

		/// <summary>
		/// Defines the update frequency in seconds
		/// </summary>
		public double UpdateFrequency { get; set; } = 0;

		/// <summary>
		/// Defines the update resolution in cycles
		/// </summary>
		public int UpdateResolution { get; set; } = 1;

		/// <summary>
		/// A list of <see cref="UpdateRequeriment"/> for this update function
		/// </summary>
		public IList<UpdateRequeriment> Requeriments { get; set; }

		/// <summary>
		/// Lazily allocated struct that contains the necessary data for a update function that is registered.
		/// </summary>
		public RegistryInfo InternalData { get; set; }

		/// <summary>
		/// Adds the update function to the primary list of update functions.
		/// </summary>
		/// <param name="updateRoot">update root to place this update function in</param>
		public void RegisterUpdateFunction(UpdateRoot updateRoot) { }

		/// <summary>
		/// Removes the update function from the primary list of update functions.
		/// </summary>
		public void UnRegisterUpdateFunction() { }

		/// <summary>
		/// Gets if the update function is currently registered.
		/// </summary>
		/// <returns>true if the update function is registered</returns>
		public bool IsUpdateFunctionRegistered() => InternalData != null && InternalData.Registered;

		/// <summary>
		/// Enables or disables this update function.
		/// </summary>
		/// <param name="enabled">Enable value</param>
		public void SetUpdateFunctionEnable(bool enabled) { CanUpdate = enabled; State = enabled ? UpdateState.Enabled : UpdateState.Disabled; }

		/// <summary>
		/// Returns whether the update function is currently enabled.
		/// </summary>
		/// <returns>True if the update function is enabled, otherwise false</returns>
		public bool IsUpdateFunctionEnabled() { return State != UpdateState.Disabled; }

		/// <summary>
		/// Returns whether it is valid to access this update function's completion handle.
		/// </summary>
		/// <returns></returns>
		public bool IsCompletionHandleValid() { return InternalData != null && InternalData.Task != null && !InternalData.Task.IsCompleted && !InternalData.Task.IsFaulted && !InternalData.Task.IsCanceled; }

		/////** Update update interval in the system and overwrite the current cooldown if any. */
		//public void UpdateUpdateIntervalAndCoolDown(float NewUpdateInterval) { }

		/////**
		//* Gets the current completion handle of this update function, so it can be delayed until a later point when some additional
		//* tasks have been completed.Only valid after TG_PreAsyncWork has started and then only until the UpdateFunction finishes
		//* execution
		//**/
		//public GraphEventRef GetCompletionHandle() { return null; }

		/////** 
		//* Gets the action update group that this function will be elligible to start in.
		//* Only valid after TG_PreAsyncWork has started through the end of the frame.
		//**/
		//public UpdateGroup GetActualUpdateGroup()
		//{
		//	return InternalData != null ? InternalData.ActualUpdateGroup : UpdateGroup;
		//}

		/////** 
		//* Gets the action update group that this function will be required to end in.
		//* Only valid after TG_PreAsyncWork has started through the end of the frame.
		//**/
		//public UpdateGroup GetActualEndUpdateGroup()
		//{
		//	return InternalData != null ? InternalData.ActualEndUpdateGroup : EndUpdateGroup;
		//}


		/////** 
		// * Adds a update function to the list of prerequisites...in other words, adds the requirement that TargetUpdateFunction is called before this update function is 
		// * @param TargetObject - UObject containing this update function. Only used to verify that the other pointer is still usable
		// * @param TargetUpdateFunction - Actual update function to use as a prerequisite
		// **/
		//public void AddPrerequisite(NI2SObject TargetObject, UpdateFunction TargetUpdateFunction) { }
		/////** 
		// * Removes a prerequisite that was previously added.
		// * @param TargetObject - UObject containing this update function. Only used to verify that the other pointer is still usable
		// * @param TargetUpdateFunction - Actual update function to use as a prerequisite
		// **/
		//public void RemovePrerequisite(NI2SObject TargetObject, UpdateFunction TargetUpdateFunction) { }
		/////** 
		// * Sets this function to hipri and all prerequisites recursively
		// * @param bInHighPriority - priority to set
		// **/
		//public void SetPriorityIncludingPrerequisites(bool InHighPriority) { }

		//public double GetLastUpdateGameTime() { return InternalData != null ? InternalData.LastUpdateGameTimeSeconds : -1.0; }


		/////**
		// * Queues a update function for execution from the game thread
		// * @param UpdateContext - context to update in
		// */
		//private void QueueUpdateFunction(UpdateTaskSequencer TTS, UpdateContext UpdateContext) { }

		/////**
		// * Queues a update function for execution from the game thread
		// * @param UpdateContext - context to update in
		// * @param StackForCycleDetection - Stack For Cycle Detection
		// */
		//private void QueueUpdateFunctionParallel(UpdateContext UpdateContext, IList<UpdateFunction> StackForCycleDetection) { }

		/////** Returns the delta time to use when updating this function given the UpdateContext */
		//private float CalculateDeltaTime(UpdateContext UpdateContext) { return -1; }

		/////** 
		// * Logs the prerequisites
		// */
		//private void ShowPrerequistes(int Indent = 1) { }

		/////** 
		// * Abstract function actually execute the update. 
		// * @param DeltaTime - frame time to advance, in seconds
		// * @param UpdateType - kind of update for this frame
		// * @param CurrentThread - thread we are executing on, useful to pass along as new tasks are created
		// * @param MyCompletionGraphEvent - completion event for this task. Useful for holding the completetion of this task until certain child tasks are complete.
		// **/
		//protected abstract void ExecuteUpdate(float DeltaTime, LevelUpdate UpdateType, ThreadType CurrentThread, GraphEventRef MyCompletionGraphEvent);
		/////** Abstract function to describe this update. Used to print messages about illegal cycles in the dependency graph **/
		//protected abstract string DiagnosticMessage();
		/////** Function to give a 'context' for this update, used for grouped active update reporting */
		//protected virtual string DiagnosticContext(bool Detailed) { return Constants.NAME_None; }

		//friend class FUpdateTaskSequencer;
		//friend class FUpdateTaskManager;
		//friend class FUpdateTaskLevel;
		//friend class FUpdateFunctionTask;
	}
}

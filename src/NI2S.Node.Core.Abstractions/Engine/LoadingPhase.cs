namespace NI2S.Node.Engine
{
    /// <summary>
    /// Engine startup loading phases.
    /// </summary>
    public enum LoadingPhase : int
    {
        /// <summary>
        /// Engine core modules load phase.
        /// </summary>
        /// <remarks>
        /// That phase is <b>only</b> intended for:
        /// <list type="bullet">
        /// <item>Replace engine core modules.</item>
        /// <item>Load NI2S.Node.Sdk (node-only) user modules.</item>
        /// <item>Load third party dependencies required later by other modules.</item>
        /// </list>   
        /// Engine core modules are loaded just after <see cref="PreLoading"/>, replaced modules will not (they are already loaded). Notice that any core engine module
        /// dependency will fail to load in <see cref="PreLoading"/>.
        /// </remarks>
        PreLoading,
        /// <summary>
        /// Clean engine user modules load phase.
        /// </summary>
        /// <remarks>
        /// Before <see cref="PreDefault"/>, engine modules are loaded and configured. Any module that only references engine modules can be loaded here.
        /// Useful to ensure that a module dependency is loaded before other modules.
        /// </remarks>
        PreDefault,
        /// <summary>
        /// Default user modules load phase.
        /// </summary>
        /// <remarks>
        /// Any shipped module can be loaded here. Notice that any module dependencies and third party libraries <b>must be</b> loaded before <see cref="Default"/>.
        /// </remarks>
        Default,
        /// <summary>
        /// High dependent user modules load phase.
        /// </summary>
        /// <remarks>
        /// Useful to ensure that every other dependencies are loaded before. After <see cref="PostDefault"/>, an <see cref="IEngineContext"/> instance will be
        /// fully configured and ready to be loaded.
        /// </remarks>
        PostDefault,
        /// <summary>
        /// World content modules load phase.
        /// </summary>
        /// <remarks>
        /// That phase is <b>only</b> intended for:
        /// <list type="bullet">
        /// <item>Content pipeline modules.</item>
        /// <item>Plugin content.</item>
        /// <item>Custom world tools.</item>
        /// </list>   
        /// Before <see cref="PostLoading"/> engine content is loaded. World content modules and plugin content are loaded after <see cref="PostLoading"/>.
        /// </remarks>
        PostLoading
    }
}

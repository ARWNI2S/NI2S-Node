using System.Runtime.CompilerServices;

namespace ARWNI2S.Node.Core.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the DragonCorp engine.
    /// </summary>
    public partial class EngineContext
    {
        #region Methods

        /// <summary>
        /// Create a static instance of the DragonCorp engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            //create ServerEngine as engine
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new NodeEngine());
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngine engine)
        {
            Singleton<IEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton DragonCorp engine used to access DragonCorp services.
        /// </summary>
        public static IEngine Current
        {
            get
            {
                if (Singleton<IEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngine>.Instance;
            }
        }

        #endregion
    }
}

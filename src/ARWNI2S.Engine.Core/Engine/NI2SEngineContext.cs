using System.Runtime.CompilerServices;
using ARWNI2S.Infrastructure;

namespace ARWNI2S.Engine
{
    /// <summary>
    /// Provides access to the singleton instance of the Node Engine.
    /// </summary>
    public partial class NI2SEngineContext
    {
        #region Methods

        /// <summary>
        /// Create a static instance of the ARWNI2S engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static INiisEngine Create()
        {
            //create NodeEngine as engine
            return Singleton<INiisEngine>.Instance ?? (Singleton<INiisEngine>.Instance = new NodeEngine());
        }

        /// <summary>
        /// Sets the static engine instance to the supplied engine. Use this method to supply your own engine implementation.
        /// </summary>
        /// <param name="engine">The engine to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(INiisEngine engine)
        {
            Singleton<INiisEngine>.Instance = engine;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton ARWNI2S engine used to access ARWNI2S services.
        /// </summary>
        public static INiisEngine Current
        {
            get
            {
                if (Singleton<INiisEngine>.Instance == null)
                {
                    Create();
                }

                return Singleton<INiisEngine>.Instance;
            }
        }

        #endregion
    }
}

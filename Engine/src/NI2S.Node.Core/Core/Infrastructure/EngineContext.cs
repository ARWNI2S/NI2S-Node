// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System.Runtime.CompilerServices;

namespace NI2S.Node.Core.Infrastructure
{
    /// <summary>
    /// Provides access to the singleton instance of the NI2S engine.
    /// </summary>
    public partial class EngineContext
    {
        #region Methods

        /// <summary>
        /// Create a static instance of the NI2S engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngine Create()
        {
            //create NI2SEngine as engine
            return Singleton<IEngine>.Instance ?? (Singleton<IEngine>.Instance = new NI2SEngine());
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
        /// Gets the singleton NI2S engine used to access NI2S services.
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

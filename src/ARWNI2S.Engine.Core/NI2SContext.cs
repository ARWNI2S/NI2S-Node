using ARWNI2S.Engine;
using ARWNI2S.Infrastructure;
using System.Runtime.CompilerServices;

namespace ARWNI2S
{
    /// <summary>
    /// Provides access to the singleton instance of the node engine context.
    /// </summary>
    public partial class NI2SContext
    {
        #region Methods

        /// <summary>
        /// Create a static instance of the ARWNI2S engine.
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public static IEngineContext Create()
        {
            //create ServerEngine as engine
            return Singleton<IEngineContext>.Instance ?? (Singleton<IEngineContext>.Instance = new EngineContext());
        }

        /// <summary>
        /// Sets the static engine context instance to the supplied engine. Use this method to supply your own engine context implementation.
        /// </summary>
        /// <param name="context">The engine context to use.</param>
        /// <remarks>Only use this method if you know what you're doing.</remarks>
        public static void Replace(IEngineContext context)
        {
            Singleton<IEngineContext>.Instance = context;
        }

        #endregion

        #region Properties

        /// <summary>
        /// Gets the singleton ARWNI2S engine used to access ARWNI2S services.
        /// </summary>
        public static IEngineContext Current
        {
            get
            {
                if (Singleton<IEngineContext>.Instance == null)
                {
                    Create();
                }

                return Singleton<IEngineContext>.Instance;
            }
        }

        #endregion
    }
}

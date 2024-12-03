//*************************************************************************************
// File:        DebugTimer.cs
//*************************************************************************************
// Description: Static timer for debugging stats purposes
//*************************************************************************************
// Classes:     DebugTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************


//*************************************************************************************
// File:        DebugTimer.cs
//*************************************************************************************
// Description: Static timer for debugging stats purposes
//*************************************************************************************
// Classes:     DebugTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************


//*************************************************************************************
// File:        DebugTimer.cs
//*************************************************************************************
// Description: Static timer for debugging stats purposes
//*************************************************************************************
// Classes:     DebugTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************


//*************************************************************************************
// File:        DebugTimer.cs
//*************************************************************************************
// Description: Static timer for debugging stats purposes
//*************************************************************************************
// Classes:     DebugTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************

using ARWNI2S.Infrastructure.Interop;

namespace ARWNI2S.Timing
{
    /// <summary>
    /// Static timer for debugging stats purposes
    /// </summary>
    internal class DebugTimer
    {
        #region Locals
        private static bool isUsingQPF;
        private static long ticksPerSecond;
        private static long lastElapsedTime;
        private static long baseTime;
        #endregion

        private DebugTimer() { }   //NOT CREATABLE

        /// <summary>
        /// Constructor.
        /// </summary>
        static DebugTimer()
        {
            ticksPerSecond = 0;
            lastElapsedTime = 0;
            baseTime = 0;

            isUsingQPF = NativeMethods.QueryPerformanceFrequency(out ticksPerSecond);

            if (isUsingQPF)
            {
                long time = 0;
                NativeMethods.QueryPerformanceCounter(out time);
                baseTime = time;
                lastElapsedTime = time;
            }
        }

        /// <summary>
        /// Metodo para obtener el tiempo absoluto desde que se inicio el sistema, en segundos.
        /// </summary>
        /// <returns>
        /// El tiempo transcurrido desde que se inicio el sistema, en segundos.
        /// </returns>
        public static double GetAbsoluteTime()
        {
            if (!isUsingQPF) return -1.0; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            long time = 0;

            NativeMethods.QueryPerformanceCounter(out time);

            double absolueTime = time / (double)ticksPerSecond;
            return absolueTime;
        }

        /// <summary>
        /// Metodo para obtener el tiempo actual modificado segun paradas y avances, en segundos.
        /// </summary>
        /// <returns>
        /// El tiempo actual del temporizador, en segundos.
        /// </returns>
        public static double GetTime()
        {
            if (!isUsingQPF) return -1.0; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            long time = 0;
            NativeMethods.QueryPerformanceCounter(out time);

            double appTime = (time - baseTime) / (double)ticksPerSecond;
            return appTime;
        }
        /// <summary>
        /// Metodo para obtener el tiempo transcurrido entre dos llamadas GetElapsedTime(), modificado segun paradas y
        /// avances, en segundos.
        /// </summary>
        /// <returns>
        /// El tiempo transcurrido desde la ultima llamada GetElapsedTime(), en segundos.
        /// </returns>
        public static double GetElapsedTime()
        {
            if (!isUsingQPF) return -1.0; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            long time = 0;

            NativeMethods.QueryPerformanceCounter(out time);

            double elapsedTime = (time - lastElapsedTime) / (double)ticksPerSecond;
            lastElapsedTime = time;
            return elapsedTime;
        }
    }
}

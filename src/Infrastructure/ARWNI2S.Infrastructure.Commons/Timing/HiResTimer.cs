//*************************************************************************************
// File:     HiResTimer.cs
//*************************************************************************************
// Description: Encapsula un temporizador de alta resolucion.
//*************************************************************************************
// Classes:      HiResTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************


//*************************************************************************************
// File:     HiResTimer.cs
//*************************************************************************************
// Description: Encapsula un temporizador de alta resolucion.
//*************************************************************************************
// Classes:      HiResTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************

using ARWNI2S.Infrastructure.Interop;

namespace ARWNI2S.Infrastructure.Timing
{
    /// <summary>
    /// Representa un temporizador de alta resolucion.
    /// </summary>
    internal class HiResTimer
    {
        #region Locals
        private bool _isUsingQPF;
        private bool _isTimerStopped;
        private ulong _ticksPerSecond;
        private ulong _stopTime;
        private ulong _lastElapsedTime;
        private ulong _baseTime;
        #endregion

        #region Propiedad
        /// <summary>
        /// Obtiene True si el temporizador esta detenido.
        /// </summary>
        public bool IsStopped { get { return _isTimerStopped; } }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public HiResTimer()
        {
            _isTimerStopped = true;
            _ticksPerSecond = 0;
            _stopTime = 0;
            _lastElapsedTime = 0;
            _baseTime = 0;

            //Usar QueryPerformanceFrequency para obtener la frecuencia del temporizador
            _isUsingQPF = NativeMethods.QueryPerformanceFrequency(out _ticksPerSecond);
        }

        /// <summary>
        /// Metodo para resetear el temporizador a 0 sin alterar si estado actual.
        /// </summary>
        public void Reset()
        {
            if (!_isUsingQPF) return; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            ulong time = 0;

            if (_stopTime != 0)
                time = _stopTime;
            else
                NativeMethods.QueryPerformanceCounter(out time);

            _baseTime = time;
            _lastElapsedTime = time;
            _stopTime = 0;
            _isTimerStopped = false;
        }

        /// <summary>
        /// Inicia el temporizador.
        /// </summary>
        public void Start()
        {
            if (!_isUsingQPF) return; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            ulong time = 0;

            if (_stopTime != 0)
                time = _stopTime;
            else
                NativeMethods.QueryPerformanceCounter(out time);


            if (_isTimerStopped) _baseTime += time - _stopTime;
            _stopTime = 0;
            _lastElapsedTime = time;
            _isTimerStopped = false;
        }

        /// <summary>
        /// Metodo para detener (or pausar) el temporizador.
        /// </summary>
        public void Stop()
        {
            if (!_isUsingQPF)
                return; //Nada que hacer

            if (!_isTimerStopped)
            {
                //Obtener el tiempo actual o el de parada
                ulong time = 0;

                if (_stopTime != 0)
                    time = _stopTime;
                else
                    NativeMethods.QueryPerformanceCounter(out time);

                _stopTime = time;
                _lastElapsedTime = time;
                _isTimerStopped = true;
            }
        }

        /// <summary>
        /// Metodo para obtener el tiempo absoluto desde que se inicio el sistema, en milisegundos.
        /// </summary>
        /// <returns>
        /// El tiempo transcurrido desde que se inicio el temporizador, en milisegundos.
        /// </returns>
        public double GetAbsoluteTimeMs()
        {
            if (!_isUsingQPF) return -1.0; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            ulong time = 0;

            if (_stopTime != 0)
                time = _stopTime;
            else
                NativeMethods.QueryPerformanceCounter(out time);

            double absolueTime = time / (double)_ticksPerSecond;
            return absolueTime * 1000.0; //en milisegundos.
        }

        /// <summary>
        /// Metodo para obtener el tiempo actual modificado segun paradas y avances, en milisegundos.
        /// </summary>
        /// <returns>
        /// El tiempo actual del temporizador, en milisegundos.
        /// </returns>
        public double GetTimeMs()
        {
            if (!_isUsingQPF) return -1.0; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            ulong time = 0;
            if (_stopTime != 0)
                time = _stopTime;
            else
                NativeMethods.QueryPerformanceCounter(out time);

            double appTime = (time - _baseTime) / (double)_ticksPerSecond;
            return appTime * 1000.0; //en milisegundos.
        }

        /// <summary>
        /// Metodo para obtener el tiempo transcurrido entre dos llamadas GetElapsedTime(), modificado segun paradas y
        /// avances, en milisegundos.
        /// </summary>
        /// <returns>
        /// El tiempo transcurrido desde la ultima llamada GetElapsedTime(), en milisegundos.
        /// </returns>
        public double GetElapsedTimeMs()
        {
            if (!_isUsingQPF) return -1.0; //Nada que hacer

            //Obtener el tiempo actual o el de parada
            ulong time = 0;
            if (_stopTime != 0)
                time = _stopTime;
            else
                NativeMethods.QueryPerformanceCounter(out time);

            double elapsedTime = (time - _lastElapsedTime) / (double)_ticksPerSecond;
            _lastElapsedTime = time;

            return elapsedTime * 1000.0; //en milisegundos.
        }
    }
}

//*************************************************************************************
// File:        SimpleTimer.cs
//*************************************************************************************
// Description: Simple timer class, in seconds.
//*************************************************************************************
// Classes:     SimpleTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************


//*************************************************************************************
// File:        SimpleTimer.cs
//*************************************************************************************
// Description: Simple timer class, in seconds.
//*************************************************************************************
// Classes:     SimpleTimer
//*************************************************************************************
// Author:      ManOguaR
//*************************************************************************************

using ARWNI2S.Infrastructure.Interop;

namespace ARWNI2S.Infrastructure.Timing
{
    /// <summary>
    /// Simple timer class, in seconds.
    /// </summary>
    internal class SimpleTimer
    {
        #region Locals
        private bool _isUsingQPF;
        private bool _isTimerStopped;
        private long _ticksPerSecond;
        private long _stopTime;
        private long _lastElapsedTime;
        private long _baseTime;
        #endregion

        #region Properties
        /// <summary>
        /// Get if timer is running.
        /// </summary>
        public bool Running { get { return !_isTimerStopped; } }
        #endregion

        /// <summary>
        /// Constructor.
        /// </summary>
        public SimpleTimer()
        {
            _isTimerStopped = true;
            _ticksPerSecond = 0;
            _stopTime = 0;
            _lastElapsedTime = 0;
            _baseTime = 0;

            _isUsingQPF = NativeMethods.QueryPerformanceFrequency(out _ticksPerSecond);
        }

        /// <summary>
        /// Resets to 0.
        /// </summary>
        public void Reset()
        {
            if (!_isUsingQPF) return;

            long time = 0;

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
        /// Starts timer.
        /// </summary>
        public void Start()
        {
            if (!_isUsingQPF) return;

            long time = 0;

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
        /// Pauses the timer.
        /// </summary>
        public void Stop()
        {
            if (!_isUsingQPF)
                return;

            if (!_isTimerStopped)
            {
                long time = 0;
                if (_stopTime != 0)
                    time = _stopTime;
                else
#if x64
                    Win64DllImporter.QueryPerformanceCounter(out time);
#else
                    NativeMethods.QueryPerformanceCounter(out time);
#endif

                _stopTime = time;
                _lastElapsedTime = time;
                _isTimerStopped = true;
            }
        }

        /// <summary>
        /// Get absolute time since system start.
        /// </summary>
        /// <returns>
        /// Time since system start, in seconds.
        /// </returns>
        public double GetAbsoluteTime()
        {
            if (!_isUsingQPF) return -1.0;

            long time = 0;

            if (_stopTime != 0)
                time = _stopTime;
            else
                NativeMethods.QueryPerformanceCounter(out time);

            double absolueTime = time / (double)_ticksPerSecond;
            return absolueTime;
        }
        /// <summary>
        /// Gets time.
        /// </summary>
        /// <returns>
        /// Actual time, in seconds.
        /// </returns>
        public double GetTime()
        {
            if (!_isUsingQPF) return -1.0;

            long time = 0;
            if (_stopTime != 0)
                time = _stopTime;
            else
                NativeMethods.QueryPerformanceCounter(out time);

            double appTime = (time - _baseTime) / (double)_ticksPerSecond;
            return appTime;
        }
        /// <summary>
        /// Gets elapsed time between two <see cout="GetElapsedTime"/> calls.
        /// </summary>
        /// <returns>
        /// Time elapsed since the last <see cout="GetElapsedTime"/> call, in seconds.
        /// </returns>
        public double GetElapsedTime()
        {
            if (!_isUsingQPF) return -1.0;

            long time = 0;
            if (_stopTime != 0) time = _stopTime;
            else
#if x64
                Win64DllImporter.QueryPerformanceCounter(out time);
#else
                NativeMethods.QueryPerformanceCounter(out time);
#endif
            double elapsedTime = (time - _lastElapsedTime) / (double)_ticksPerSecond;
            _lastElapsedTime = time;

            return elapsedTime;
        }
    }
}

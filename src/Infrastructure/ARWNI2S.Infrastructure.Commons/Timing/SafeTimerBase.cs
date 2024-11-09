using ARWNI2S.Infrastructure.Logging;
using ARWNI2S.Infrastructure.Resources;
using Microsoft.Extensions.Logging;

namespace ARWNI2S.Infrastructure.Timing
{
    /// <summary>
    /// SafeTimerBase - an public base class for implementing sync and async timers in NI2S.
    /// </summary>
    internal class SafeTimerBase : IDisposable
    {
        private Timer timer;
        private Func<object, Task> asynTaskCallback;
        private TimerCallback syncCallbackFunc;
        private TimeSpan dueTime;
        private TimeSpan timerFrequency;
        private bool timerStarted;
        private DateTime previousTickTime;
        private int totalNumTicks;
        private ILogger logger;

        public SafeTimerBase(Func<object, Task> asynTaskCallback, object state, ILogger logger = null)
        {
            this.logger = logger;
            Init(asynTaskCallback, null, state, Constants.INFINITE_TIMESPAN, Constants.INFINITE_TIMESPAN);
        }

        public SafeTimerBase(Func<object, Task> asynTaskCallback, object state, TimeSpan dueTime, TimeSpan period, ILogger logger = null)
        {
            this.logger = logger;
            Init(asynTaskCallback, null, state, dueTime, period);
            Start(dueTime, period);
        }

        public SafeTimerBase(TimerCallback syncCallback, object state, ILogger logger = null)
        {
            this.logger = logger;
            Init(null, syncCallback, state, Constants.INFINITE_TIMESPAN, Constants.INFINITE_TIMESPAN);
        }

        public SafeTimerBase(TimerCallback syncCallback, object state, TimeSpan dueTime, TimeSpan period, ILogger logger = null)
        {
            this.logger = logger;
            Init(null, syncCallback, state, dueTime, period);
            Start(dueTime, period);
        }

        public void Start(TimeSpan due, TimeSpan period)
        {
            if (timerStarted)
                throw new InvalidOperationException(string.Format(ErrorStrings.SafeTimerBase_Start_InvalidOperationException_Format, GetFullName()));
            if (period == TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period), period, ErrorStrings.SafeTimerBase_TimeSpan_ArgumentOutOfRangeException_Format);

            timerFrequency = period;
            dueTime = due;
            timerStarted = true;
            previousTickTime = DateTime.UtcNow;
            timer.Change(due, Constants.INFINITE_TIMESPAN);
        }

        private void Init(Func<object, Task> asynCallback, TimerCallback synCallback, object state, TimeSpan due, TimeSpan period)
        {
            if (synCallback == null && asynCallback == null)
                throw new ArgumentNullException(nameof(synCallback), ErrorStrings.SafeTimerBase_Init_ArgumentNullException_Message);

            int numNonNulls = (asynCallback != null ? 1 : 0) + (synCallback != null ? 1 : 0);

            if (numNonNulls > 1)
                throw new ArgumentNullException(nameof(synCallback), ErrorStrings.SafeTimerBase_Init_InvalidOperationException_Message);
            if (period == TimeSpan.Zero)
                throw new ArgumentOutOfRangeException(nameof(period), period, ErrorStrings.SafeTimerBase_TimeSpan_ArgumentOutOfRangeException_Format);

            asynTaskCallback = asynCallback;
            syncCallbackFunc = synCallback;
            timerFrequency = period;
            dueTime = due;
            totalNumTicks = 0;

            logger.LogDebug(ErrorCode.TimerChanging, LocalizedStrings.TimerChanging_Creating, GetFullName(), due, period);

            timer = new Timer(HandleTimerCallback, state, Constants.INFINITE_TIMESPAN, Constants.INFINITE_TIMESPAN);
        }

        #region IDisposable Members

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        // Maybe called by finalizer thread with disposing=false. As per guidelines, in such a case do not touch other objects.
        // Dispose() may be called multiple times
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                DisposeTimer();
            }
        }

        public void DisposeTimer()
        {
            if (timer != null)
            {
                try
                {
                    var t = timer;
                    timer = null;
                    logger.LogDebug(ErrorCode.TimerDisposing, "Disposing timer {0}", GetFullName());
                    t.Dispose();

                }
                catch (Exception exc)
                {
                    logger.LogWarning(ErrorCode.TimerDisposeError,
                        string.Format("Ignored error disposing timer {0}", GetFullName()), exc);
                }
            }
        }

        #endregion

        private string GetFullName()
        {
            // the type information is really useless and just too long. 
            string name;
            if (syncCallbackFunc != null)
                name = "sync";
            else if (asynTaskCallback != null)
                name = "asynTask";
            else
                throw new InvalidOperationException(ErrorStrings.SafeTimerBase_GetFullName_InvalidOperationException);

            return string.Format("{0}.SafeTimerBase", name);
        }

        public bool CheckTimerFreeze(DateTime lastCheckTime, Func<string> callerName)
        {
            return CheckTimerDelay(previousTickTime, totalNumTicks,
                        dueTime, timerFrequency, logger, () => string.Format("{0}.{1}", GetFullName(), callerName()), ErrorCode.Timer_SafeTimerIsNotTicking, true);
        }

        public static bool CheckTimerDelay(DateTime previousTickTime, int totalNumTicks,
                        TimeSpan dueTime, TimeSpan timerFrequency, ILogger logger, Func<string> getName, ErrorCode errorCode, bool freezeCheck)
        {
            TimeSpan timeSinceLastTick = DateTime.UtcNow - previousTickTime;
            TimeSpan exceptedTimeToNexTick = totalNumTicks == 0 ? dueTime : timerFrequency;
            TimeSpan exceptedTimeWithSlack;
            if (exceptedTimeToNexTick >= TimeSpan.FromSeconds(6))
            {
                exceptedTimeWithSlack = exceptedTimeToNexTick + TimeSpan.FromSeconds(3);
            }
            else
            {
                exceptedTimeWithSlack = exceptedTimeToNexTick.Multiply(1.5);
            }
            if (timeSinceLastTick <= exceptedTimeWithSlack) return true;

            // did not tick in the last period.
            var errMsg = string.Format(ErrorStrings.SafeTimerBase_CheckTimerDelay_ErrorMessage,
                freezeCheck ? ErrorStrings.SafeTimerBase_CheckTimerDelay_FreezeAlertMessage : "-", // 0
                getName == null ? "" : getName(),   // 1
                LogFormatter.PrintDate(previousTickTime), // 2
                timeSinceLastTick,                  // 3
                exceptedTimeToNexTick);             // 4

            if (freezeCheck)
            {
                logger.LogError(errorCode, errMsg);
            }
            else
            {
                logger.LogWarning(errorCode, errMsg);
            }
            return false;
        }

        /// <summary>
        /// Changes the start time and the interval between method invocations for a timer, using TimeSpan values to measure time intervals.
        /// </summary>
        /// <param name="newDueTime">A TimeSpan representing the amount of time to delay before invoking the callback method specified when the Timer was constructed. Specify negative one (-1) milliseconds to prevent the timer from restarting. Specify zero (0) to restart the timer immediately.</param>
        /// <param name="period">The time interval between invocations of the callback method specified when the Timer was constructed. Specify negative one (-1) milliseconds to disable periodic signaling.</param>
        /// <returns><c>true</c> if the timer was successfully updated; otherwise, <c>false</c>.</returns>
        private bool Change(TimeSpan newDueTime, TimeSpan period)
        {
            if (period == TimeSpan.Zero) throw new ArgumentOutOfRangeException(nameof(period), period, string.Format("Cannot use TimeSpan.Zero for timer {0} period", GetFullName()));

            if (timer == null) return false;

            timerFrequency = period;

            logger.LogDebug(ErrorCode.TimerChanging, LocalizedStrings.TimerChanging, GetFullName(), newDueTime, period);

            try
            {
                // Queue first new timer tick
                return timer.Change(newDueTime, Constants.INFINITE_TIMESPAN);
            }
            catch (Exception exc)
            {
                logger.LogWarning(ErrorCode.TimerChangeError,
                    string.Format(ErrorStrings.TimerChangeError, GetFullName()), exc);
                return false;
            }
        }

        private void HandleTimerCallback(object state)
        {
            if (timer == null) return;

            if (asynTaskCallback != null)
            {
                HandleAsyncTaskTimerCallback(state);
            }
            else
            {
                HandleSyncTimerCallback(state);
            }
        }

        private void HandleSyncTimerCallback(object state)
        {
            try
            {
                logger.LogDebug(ErrorCode.TimerBeforeCallback, LocalizedStrings.TimerBeforeCallbackSync, GetFullName());
                syncCallbackFunc(state);
                logger.LogDebug(ErrorCode.TimerAfterCallback, LocalizedStrings.TimerAfterCallbackSync, GetFullName());
            }
            catch (Exception exc)
            {
                logger.LogWarning(ErrorCode.TimerCallbackError, string.Format(ErrorStrings.TimerCallbackErrorSync, exc.Message, GetFullName()), exc);
            }
            finally
            {
                previousTickTime = DateTime.UtcNow;
                // Queue next timer callback
                QueueNextTimerTick();
            }
        }

        private async void HandleAsyncTaskTimerCallback(object state)
        {
            if (timer == null) return;

            // There is a subtle race/issue here w.r.t unobserved promises.
            // It may happen than the asyncCallbackFunc will resolve some promises on which the higher level application code is depends upon
            // and this promise's await or CW will fire before the below code (after await or Finally) even runs.
            // In the unit test case this may lead to the situation where unit test has finished, but p1 or p2 or p3 have not been observed yet.
            // To properly fix this we may use a mutex/monitor to delay execution of asyncCallbackFunc until all CWs and Finally in the code below were scheduled 
            // (not until CW lambda was run, but just until CW function itself executed). 
            // This however will relay on scheduler executing these in separate threads to prevent deadlock, so needs to be done carefully. 
            // In particular, need to make sure we execute asyncCallbackFunc in another thread (so use StartNew instead of ExecuteWithSafeTryCatch).

            try
            {
                logger.LogDebug(ErrorCode.TimerBeforeCallback, LocalizedStrings.TimerBeforeCallbackAsync, GetFullName());
                await asynTaskCallback(state);
                logger.LogDebug(ErrorCode.TimerAfterCallback, LocalizedStrings.TimerAfterCallbackAsync, GetFullName());
            }
            catch (Exception exc)
            {
                logger.LogWarning(ErrorCode.TimerCallbackError, string.Format(ErrorStrings.TimerCallbackErrorAsync, exc.Message, GetFullName()), exc);
            }
            finally
            {
                previousTickTime = DateTime.UtcNow;
                // Queue next timer callback
                QueueNextTimerTick();
            }
        }

        private void QueueNextTimerTick()
        {
            try
            {
                if (timer == null) return;

                totalNumTicks++;

                logger.LogDebug(ErrorCode.TimerChanging, LocalizedStrings.TimerChanging_Queue, GetFullName());

                if (timerFrequency == Constants.INFINITE_TIMESPAN)
                {
                    //timer.Change(Constants.INFINITE_TIMESPAN, Constants.INFINITE_TIMESPAN);
                    DisposeTimer();

                    logger.LogDebug(ErrorCode.TimerStopped, LocalizedStrings.TimerStopped, GetFullName());
                }
                else
                {
                    timer.Change(timerFrequency, Constants.INFINITE_TIMESPAN);

                    logger.LogDebug(ErrorCode.TimerNextTick, LocalizedStrings.TimerNextTick, GetFullName(), timerFrequency);
                }
            }
            catch (ObjectDisposedException ode)
            {
                logger.LogWarning(ErrorCode.TimerDisposeError,
                    string.Format(ErrorStrings.TimerDisposeError, GetFullName()), ode);
            }
            catch (Exception exc)
            {
                logger.LogError(ErrorCode.TimerQueueTickError,
                    string.Format(ErrorStrings.TimerQueueTickError, GetFullName()), exc);
            }
        }
    }
}
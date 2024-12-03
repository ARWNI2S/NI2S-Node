namespace ARWNI2S.Diagnostics
{
    public enum LogCode : int
    {
        Timer = 9000,
        TimerChanging = Timer + 1,
        TimerBeforeCallback = Timer + 2,
        TimerAfterCallback = Timer + 3,
        TimerNextTick = Timer + 4,
        TimerStopped = Timer + 5,
        TimerDisposing = Timer + 9,
        TimerError = Timer + 100,
        Timer_SafeTimerIsNotTicking = TimerError + 1,
        TimerCallbackError = TimerError + 2,
        TimerChangeError = TimerError + 3,
        TimerQueueTickError = TimerError + 4,
        TimerDisposeError = TimerError + 9,

        Lifecycle = 10000,
        NodeStartPerfMeasure = Lifecycle + 1,
        LifecycleFailure = Lifecycle + 100,
        LifecycleStartFailure = LifecycleFailure + 1,
        LifecycleStopFailure = LifecycleFailure + 2,
    }
}
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace NI2S.Node.Runtime
{
    internal class AsyncTaskSafeTimer : SafeTimerBase
    {
        public AsyncTaskSafeTimer(ILogger logger, Func<object, Task> asyncTaskCallback, object state) : base(logger, asyncTaskCallback, state)
        {
        }

        public AsyncTaskSafeTimer(ILogger logger, Func<object, Task> asyncTaskCallback, object state, TimeSpan dueTime, TimeSpan period) : base(logger, asyncTaskCallback, state, dueTime, period)
        {
        }
    }
}

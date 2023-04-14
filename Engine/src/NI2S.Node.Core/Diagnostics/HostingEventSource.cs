// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using System;
using System.Diagnostics.Tracing;
using System.Runtime.CompilerServices;
using System.Threading;

namespace NI2S.Node.Diagnostics
{
    internal sealed class HostingEventSource : EventSource
    {
        public static readonly HostingEventSource Log = new();

        private IncrementingPollingCounter _requestsPerSecondCounter;
        private PollingCounter _totalMessagesCounter;
        private PollingCounter _failedMessagesCounter;
        private PollingCounter _currentMessagesCounter;

        private long _totalMessages;
        private long _currentMessages;
        private long _failedMessages;

        internal HostingEventSource()
            : base("NI2S.Node.Hosting", EventSourceSettings.EtwManifestEventFormat)
        {
        }

        // Used for testing
        internal HostingEventSource(string eventSourceName)
            : base(eventSourceName, EventSourceSettings.EtwManifestEventFormat)
        {
        }

        // NOTE
        // - The 'Start' and 'Stop' suffixes on the following event names have special meaning in EventSource. They
        //   enable creating 'activities'.
        //   For more information, take a look at the following blog post:
        //   https://blogs.msdn.microsoft.com/vancem/2015/09/14/exploring-eventsource-activity-correlation-and-causation-features/
        // - A stop event's event id must be next one after its start event.

        [Event(1, Level = EventLevel.Informational)]
        public void HostStart()
        {
            WriteEvent(1);
        }

        [Event(2, Level = EventLevel.Informational)]
        public void HostStop()
        {
            WriteEvent(2);
        }

        [Event(3, Level = EventLevel.Informational)]
        public void MessageStart(string method, string path)
        {
            Interlocked.Increment(ref _totalMessages);
            Interlocked.Increment(ref _currentMessages);
            WriteEvent(3, method, path);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [Event(4, Level = EventLevel.Informational)]
        public void MessageStop()
        {
            Interlocked.Decrement(ref _currentMessages);
            WriteEvent(4);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        [Event(5, Level = EventLevel.Error)]
        public void UnhandledException()
        {
            WriteEvent(5);
        }

        [Event(6, Level = EventLevel.Informational)]
        public void ServerReady()
        {
            WriteEvent(6);
        }

        [NonEvent]
        internal void MessageFailed()
        {
            Interlocked.Increment(ref _failedMessages);
        }

        protected override void OnEventCommand(EventCommandEventArgs command)
        {
            if (command.Command == EventCommand.Enable)
            {
                // This is the convention for initializing counters in the RuntimeEventSource (lazily on the first enable command).
                // They aren't disabled afterwards...

                _requestsPerSecondCounter ??= new IncrementingPollingCounter("requests-per-second", this, () => Volatile.Read(ref _totalMessages))
                {
                    DisplayName = "Message Rate",
                    DisplayRateTimeScale = TimeSpan.FromSeconds(1)
                };

                _totalMessagesCounter ??= new PollingCounter("total-requests", this, () => Volatile.Read(ref _totalMessages))
                {
                    DisplayName = "Total Messages",
                };

                _currentMessagesCounter ??= new PollingCounter("current-requests", this, () => Volatile.Read(ref _currentMessages))
                {
                    DisplayName = "Current Messages"
                };

                _failedMessagesCounter ??= new PollingCounter("failed-requests", this, () => Volatile.Read(ref _failedMessages))
                {
                    DisplayName = "Failed Messages"
                };
            }
        }
    }
}

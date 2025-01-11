using ARWNI2S.Engine.Environment;
using System.Diagnostics;
using System.Reflection;

namespace ARWNI2S.Engine.Threading
{
    public static class ThreadManager
    {
        private static readonly Thread _mainThread;

        private static readonly Dictionary<Type, IThreadManager> _managers = [];

        static ThreadManager()
        {
            _mainThread = Thread.CurrentThread;
            _managers.Add(null, new DefaultThreadManager());
        }

        public static IThreadManager Default => _managers[null];
        public static IThreadManager GetManager(Type type)
        {
            Debug.Assert(type != null);
            Debug.Assert(_managers.ContainsKey(type));

            return _managers[type];
        }

        internal static IThreadManager<TProcessor> RegisterProcessor<TProcessor>(TProcessor processor)
            where TProcessor : class, INiisProcessor
        {
            Debug.Assert(processor != null);

            var type = typeof(TProcessor);

            if (_managers.ContainsKey(type))
                throw new InvalidOperationException($"Processor of type {type.Name} is already registered.");

            _managers.Add(type, new ProcessorThreadManager<TProcessor>(processor));
            return (IThreadManager<TProcessor>)_managers[type];
        }

        private class DefaultThreadManager : IThreadManager
        {
            public Thread DedicatedThread => _mainThread;

            public void Run()
            {
                DedicatedThread.Start();
            }
            public void End()
            {
            }

        }

        private class ProcessorThreadManager<TProcessor> : IThreadManager<TProcessor>
            where TProcessor : class, INiisProcessor
        {
            private readonly TProcessor _processor;

            public ProcessorThreadManager(TProcessor processor)
            {
                _processor = processor;

                var methodInfo = typeof(TProcessor).GetMethod("ThreadStartImpl", BindingFlags.NonPublic | BindingFlags.Instance)
                    ?? throw new InvalidOperationException($"Processor of type {typeof(TProcessor).Name} does not have a private method.");

                DedicatedThread = new(() => methodInfo.Invoke(_processor, null));
            }

            public Thread DedicatedThread { get; }

            public void Run()
            {
                DedicatedThread.Start();
            }

            public void End()
            {
            }

        }
    }
}
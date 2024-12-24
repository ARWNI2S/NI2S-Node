using System.Collections;

namespace ARWNI2S.Engine.Runtime
{
    public class UpdateRing : IEnumerable<Task>
    {
        private readonly Enumerator _enumerator = new();

        private UpdateRoot frameRoot;
        private UpdateRoot currentCycle;

        public FrameId Frame => frameRoot.FrameId;
        public CycleId Cycle => currentCycle.CycleId;

        IEnumerator<Task> IEnumerable<Task>.GetEnumerator() => _enumerator;

        IEnumerator IEnumerable.GetEnumerator() => _enumerator;

        private class Enumerator : IEnumerator<Task>, IEnumerator
        {
            public Task Current => throw new NotImplementedException();

            object IEnumerator.Current => throw new NotImplementedException();

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                return true;
            }

            public void Reset()
            {
            }
        }
    }
}
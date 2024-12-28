using System.Diagnostics;

namespace ARWNI2S.Engine.Resources.Internal
{
    internal class HandleManager<TData, TTag>
        where TData : class
        where TTag : struct
    {
        private readonly List<TData> _userData = [];
        private readonly List<int> _magicNumbers = [];
        private readonly List<int> _freeSlots = [];

        public HandleManager() { }

        public TData Acquire(ref Handle<TTag> handle)
        {
            int ix;

            if (_freeSlots.Count == 0)
            {
                ix = _magicNumbers.Count;
                handle.Init(ix);
                _userData.Add(default);
                _magicNumbers.Add(handle.Magic);
            }
            else
            {
                ix = _freeSlots[0];
                handle.Init(ix);
                _freeSlots.RemoveAt(0);
                _magicNumbers.Add(handle.Magic);
            }
            return _userData[ix];
        }

        public void Release(Handle<TTag> handle)
        {
            int ix = handle.Index;
            Debug.Assert(ix < _userData.Count);
            Debug.Assert(_magicNumbers[ix] == handle.Magic);

            _magicNumbers[ix] = 0;
            _freeSlots.Add(ix);
        }

        public TData Dereference(Handle<TTag> handle)
        {
            if (handle.IsNull()) return default!;

            int ix = handle.Index;
            if (ix >= _userData.Count || _magicNumbers[ix] != handle.Magic)
            {
                return default!;
            }
            return _userData[ix];
        }

        public int UsedHandles { get { return _magicNumbers.Count - _freeSlots.Count; } }

        public bool HasUsedHandles { get { return UsedHandles == 0; } }
    }
}

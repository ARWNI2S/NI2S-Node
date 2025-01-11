// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Diagnostics;

namespace ARWNI2S.Engine.Environment.Internal
{
    internal class HandleManager<TData, TTag>
    {
        private readonly List<TData> _userData = [];
        private readonly List<int> _magicNumbers = [];
        private readonly List<int> _freeSlots = [];

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
            if (handle.IsNull) return default!;

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

    internal class HandleManager<TData>
        : HandleManager<TData, TData>
    { }
}

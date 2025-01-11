using ARWNI2S.Engine.Core;
using System.Diagnostics;

namespace ARWNI2S.Engine.Environment.Internal
{
    internal struct Handle<TTag>
    {
        private const int MAX_BITS = 16;
        private const int MAX_VALUE = (1 << MAX_BITS) - 1;

        private static readonly object _autoLock = new();
        private static int _auto = 0;

        private int _handle;

        public readonly int Index
        {
            get => _handle & MAX_VALUE; // Extraemos los bits bajos
        }

        public readonly int Magic
        {
            get => (_handle >> MAX_BITS) & MAX_VALUE; // Extraemos los bits altos
        }

        public Handle() { _handle = 0; }

        public Handle(int value) { _handle = value; }

        public void Init(int index)
        {
            Debug.Assert(IsNull, "Handle must be null before initialization.");
            Debug.Assert(index <= MAX_VALUE, "Index exceeds allowed range.");

            lock (_autoLock)
            {
                _auto = _auto % MAX_VALUE + 1;
                _handle = (index << MAX_BITS) + _auto;
            }
        }

        public readonly int Value { get { return _handle; } }

        public readonly bool IsNull { get { return _handle == 0; } }

        // Conversión implícita a int
        public static implicit operator int(Handle<TTag> handle)
        {
            return handle._handle;
        }

        // Conversión explícita desde int
        public static explicit operator Handle<TTag>(int value)
        {
            return new Handle<TTag> { _handle = value };
        }

        // Conversión implícita a EntityId
        public static implicit operator EntityId(Handle<TTag> handle)
        {
            return (EntityId)handle._handle;
        }

        // Conversión explícita desde EntityId
        public static explicit operator Handle<TTag>(EntityId id)
        {
            return new Handle<TTag> { _handle = (int)id };
        }

        // Operadores de comparación
        public static bool operator ==(Handle<TTag> left, Handle<TTag> right)
        {
            return left._handle == right._handle;
        }

        public static bool operator !=(Handle<TTag> left, Handle<TTag> right)
        {
            return !(left == right);
        }

        public static bool operator ==(Handle<TTag> handle, int value)
        {
            return handle != null && handle._handle == value;
        }

        public static bool operator !=(Handle<TTag> handle, int value)
        {
            return !(handle == value);
        }

        public static bool operator ==(Handle<TTag> handle, EntityId id)
        {
            return handle._handle == id;
        }

        public static bool operator !=(Handle<TTag> handle, EntityId id)
        {
            return !(handle == id);
        }

        public bool Equals(EntityId entityId)
        {
            return _handle == entityId;
        }

        // Métodos sobrescritos
        public override readonly bool Equals(object obj)
        {
            if (obj is Handle<TTag> otherHandle)
            {
                return _handle == otherHandle._handle;
            }

            return false;
        }

        public override readonly int GetHashCode()
        {
            return _handle.GetHashCode();
        }

        public override readonly string ToString() =>
            $"Handle(Index: {Index}, Magic: {Magic}, RawValue: 0x{_handle:X8})";
    }
}

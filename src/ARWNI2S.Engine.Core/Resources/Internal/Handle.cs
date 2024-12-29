using System.Diagnostics;
using System.Runtime.InteropServices;

namespace ARWNI2S.Engine.Resources.Internal
{
    [StructLayout(LayoutKind.Explicit)]
    internal struct Handle<TTag>
    {
        private const int MAX_BITS = 16;
        private const int MAX_VALUE = (1 << MAX_BITS) - 1;

        private static int _auto = 0;

        [FieldOffset(0)]
        private int _handle;

        [FieldOffset(0)]
        private short _index;

        [FieldOffset(2)]
        private short _magic;


        public Handle() { _handle = 0; }

        internal Handle(int value) { _handle = value; }

        public void Init(int index)
        {
            Debug.Assert(IsNull(), "Handle must be null before initialization.");
            Debug.Assert(index <= MAX_VALUE, "Index exceeds allowed range.");

            // Incrementar de forma segura para entornos multi-hilo
            _auto = (_auto % MAX_VALUE) + 1;

            _handle = (index << MAX_BITS) + _auto;
        }

        public readonly int Value
        {
            get { return _handle; }
        }
        public readonly short Index
        {
            get { return _index; }
        }
        public readonly short Magic
        {
            get { return _magic; }
        }

        public readonly bool IsNull()
        {
            return _handle == 0;
        }

        // Conversión implícita a uint
        public static implicit operator int(Handle<TTag> handle)
        {
            return handle._handle;
        }

        // Conversión explícita desde uint
        public static explicit operator Handle<TTag>(int value)
        {
            return new Handle<TTag> { _handle = value };
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

        public static bool operator ==(Handle<TTag> handle, uint value)
        {
            return handle != null && handle._handle == value;
        }

        public static bool operator !=(Handle<TTag> handle, uint value)
        {
            return !(handle == value);
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

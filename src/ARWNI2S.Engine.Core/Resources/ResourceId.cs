using ARWNI2S.Engine.Resources.Internal;
using System.Runtime.InteropServices;

namespace ARWNI2S.Engine.Resources
{
    [StructLayout(LayoutKind.Explicit)]
    public struct ResourceId
    {
        [FieldOffset(0)]
        private int _handle;

        [FieldOffset(0)]
        private short _index;

        [FieldOffset(2)]
        private short _magic;

        public ResourceId()
        {

        }

        internal ResourceId(int handle)
        {
            _handle = handle;
        }

        internal ResourceId(short index, short magic)
        {
            _index = index;
            _magic = magic;
        }

        // Conversión implícita a int
        public static implicit operator int(ResourceId id)
        {
            return id._handle;
        }
        // Conversión explícita desde int
        public static explicit operator ResourceId(int value)
        {
            return new ResourceId { _handle = value };
        }

        // Conversión implícita a uint
        public static implicit operator uint(ResourceId id)
        {
            return (uint)id._handle;
        }
        // Conversión explícita desde uint
        public static explicit operator ResourceId(uint value)
        {
            return new ResourceId { _handle = (int)value };
        }

        // Operadores de comparación
        public static bool operator ==(ResourceId left, ResourceId right)
        {
            return left._handle == right._handle;
        }

        public static bool operator !=(ResourceId left, ResourceId right)
        {
            return !(left == right);
        }

        public static bool operator ==(ResourceId handle, uint value)
        {
            return handle != null && handle._handle == value;
        }

        public static bool operator !=(ResourceId handle, uint value)
        {
            return !(handle == value);
        }

        // Métodos sobrescritos
        public override readonly bool Equals(object obj)
        {
            if (obj is ResourceId otherHandle)
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
            $"Handle(Index: {_index}, Magic: {_magic}, RawValue: 0x{_handle:X8})";
    }

    public static class ResourceIdExtensions
    {
        internal static ResourceId ToResourceId<TTag>(this Handle<TTag> handle)
        {
            return new ResourceId((int)handle);

        }

        internal static Handle<TTag> ToHandle<TTag>(this ResourceId id)
        {
            return new Handle<TTag>((int)id);
        }
    }
}
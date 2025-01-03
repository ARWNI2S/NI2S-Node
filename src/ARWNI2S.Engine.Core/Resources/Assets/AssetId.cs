﻿using ARWNI2S.Engine.Resources.Internal;
using System.Runtime.InteropServices;

namespace ARWNI2S.Engine.Resources.Assets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct AssetId
    {
        [FieldOffset(0)]
        private int _handle;

        [FieldOffset(0)]
        private short _index;

        [FieldOffset(2)]
        private short _magic;

        public AssetId()
        {

        }

        internal AssetId(int handle)
        {
            _handle = handle;
        }

        internal AssetId(short index, short magic)
        {
            _index = index;
            _magic = magic;
        }

        // Conversión implícita a int
        public static implicit operator int(AssetId id)
        {
            return id._handle;
        }
        // Conversión explícita desde int
        public static explicit operator AssetId(int value)
        {
            return new AssetId { _handle = value };
        }

        // Conversión implícita a uint
        public static implicit operator uint(AssetId id)
        {
            return (uint)id._handle;
        }
        // Conversión explícita desde uint
        public static explicit operator AssetId(uint value)
        {
            return new AssetId { _handle = (int)value };
        }

        // Operadores de comparación
        public static bool operator ==(AssetId left, AssetId right)
        {
            return left._handle == right._handle;
        }

        public static bool operator !=(AssetId left, AssetId right)
        {
            return !(left == right);
        }

        public static bool operator ==(AssetId handle, uint value)
        {
            return handle != null && handle._handle == value;
        }

        public static bool operator !=(AssetId handle, uint value)
        {
            return !(handle == value);
        }

        // Métodos sobrescritos
        public override readonly bool Equals(object obj)
        {
            if (obj is AssetId otherHandle)
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

    public static class AssetIdExtensions
    {
        internal static AssetId ToAssetId<TTag>(this Handle<TTag> handle)
        {
            return new AssetId((int)handle);

        }

        internal static Handle<TTag> ToHandle<TTag>(this AssetId id)
        {
            return new Handle<TTag>((int)id);
        }

        internal static AssetId ToAssetId<TTag>(this ResourceId id)
        {
            return new AssetId((int)id);

        }

        internal static ResourceId ToResourceIde<TTag>(this AssetId id)
        {
            return new ResourceId((int)id);
        }
    }
}
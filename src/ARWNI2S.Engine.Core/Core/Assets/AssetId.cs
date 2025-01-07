using ARWNI2S.Engine.Environment.Internal;
using System.Runtime.InteropServices;

namespace ARWNI2S.Engine.Core
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct AssetId
    {
        [FieldOffset(0)]
        private readonly int _identity;

        public static readonly AssetId Null = new(0);

        private AssetId(int value)
        {
            _identity = value;
        }

        //public readonly int Value => _identity;

        public readonly bool IsNull => _identity == 0;

        // Implicit conversion
        public static implicit operator int(AssetId id) => id._identity;
        public static implicit operator uint(AssetId id) => unchecked((uint)id._identity);
        public static implicit operator long(AssetId id) => (long)id._identity << 32;
        public static implicit operator ulong(AssetId id) => (ulong)(uint)id._identity << 32;
        public static implicit operator byte[](AssetId id) => BitConverter.GetBytes(id._identity);
        public static implicit operator bool[](AssetId id)
        {
            var bits = new bool[32];
            for (int i = 0; i < 32; i++)
                bits[i] = (id._identity & (1 << i)) != 0;
            return bits;
        }
        public static implicit operator string(AssetId id) => $"0x{id._identity:X8}";

        // Explicit conversion
        public static explicit operator AssetId(int value)
        {
            if (value == 0)
                throw new ArgumentException("AssetId value cannot be zero. Use AssetId.Null instead.");
            return new AssetId(value);
        }
        public static explicit operator AssetId(uint value)
        {
            if (value == 0)
                throw new ArgumentException("AssetId value cannot be zero. Use AssetId.Null instead.");
            return new AssetId((int)value);
        }
        public static explicit operator AssetId(long value)
        {
            int intValue = (int)(value >> 32);
            if (intValue == 0)
                throw new ArgumentException("AssetId value cannot be zero. Use AssetId.Null instead.");
            return new AssetId(intValue);
        }
        public static explicit operator AssetId(ulong value)
        {
            int intValue = (int)(value >> 32);
            if (intValue == 0)
                throw new ArgumentException("AssetId value cannot be zero. Use AssetId.Null instead.");
            return new AssetId(intValue);
        }
        public static explicit operator AssetId(byte[] value)
        {
            if (value == null || value.Length != 4)
                throw new ArgumentException("Invalid byte array length for AssetId. Expected 4 bytes.");
            int intValue = BitConverter.ToInt32(value, 0);
            if (intValue == 0)
                throw new ArgumentException("AssetId value cannot be zero. Use AssetId.Null instead.");
            return new AssetId(intValue);
        }
        public static explicit operator AssetId(bool[] value)
        {
            if (value == null || value.Length != 32)
                throw new ArgumentException("Invalid bool array length for AssetId. Expected 32 booleans.");
            int result = 0;
            for (int i = 0; i < 32; i++)
                if (value[i])
                    result |= (1 << i);
            if (result == 0)
                throw new ArgumentException("AssetId value cannot be zero. Use AssetId.Null instead.");
            return new AssetId(result);
        }
        public static explicit operator AssetId(string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("0x"))
                throw new ArgumentException("Invalid string format for AssetId. Expected hexadecimal starting with 0x.");
            if (!int.TryParse(value[2..], System.Globalization.NumberStyles.HexNumber, null, out int parsedValue) || parsedValue == 0)
                throw new ArgumentException("Invalid or zero hexadecimal value for AssetId. Use AssetId.Null instead.");
            return new AssetId(parsedValue);
        }

        // Operators
        public static bool operator ==(AssetId left, AssetId right) => left._identity == right._identity;
        public static bool operator !=(AssetId left, AssetId right) => !(left == right);
        public static bool operator ==(AssetId id, int value) => id._identity == value;
        public static bool operator !=(AssetId id, int value) => !(id == value);
        public static bool operator ==(AssetId id, uint value) => id._identity == unchecked((int)value);
        public static bool operator !=(AssetId id, uint value) => !(id == value);
        public static bool operator ==(AssetId id, long value) => id._identity == (int)(value >> 32);
        public static bool operator !=(AssetId id, long value) => !(id == value);
        public static bool operator ==(AssetId id, ulong value) => id._identity == (int)(value >> 32);
        public static bool operator !=(AssetId id, ulong value) => !(id == value);
        public static bool operator ==(AssetId id, byte[] value)
        {
            if (value == null || value.Length != 4)
                return false;
            return id._identity == BitConverter.ToInt32(value, 0);
        }
        public static bool operator !=(AssetId id, byte[] value) => !(id == value);
        public static bool operator ==(AssetId id, bool[] value)
        {
            if (value == null || value.Length != 32)
                return false;
            int result = 0;
            for (int i = 0; i < 32; i++)
                if (value[i])
                    result |= (1 << i);
            return id._identity == result;
        }
        public static bool operator !=(AssetId id, bool[] value) => !(id == value);
        public static bool operator ==(AssetId id, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !value.StartsWith("0x"))
                return false;
            if (!int.TryParse(value[2..], System.Globalization.NumberStyles.HexNumber, null, out int parsedValue))
                return false;
            return id._identity == parsedValue;
        }
        public static bool operator !=(AssetId id, string value) => !(id == value);

        internal bool Equals<TTag>(Handle<TTag> handle)
        {
            return _identity == handle;
        }

        // Overrides
        public override bool Equals(object obj) => obj is AssetId other && _identity == other._identity;
        public override int GetHashCode() => _identity.GetHashCode();
        public override string ToString() => $"AssetId(Raw: 0x{_identity:X8})";
    }
}

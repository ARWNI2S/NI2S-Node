using System.Globalization;

namespace ARWNI2S.Engine.Core
{
    public readonly struct EntityId
    {
        private readonly object _inner; // Almacena los bits (int, long, Guid, etc.)

        private EntityId(object inner)
        {
            _inner = inner;
        }

        // Operadores implícitos y explícitos

        // Desde Guid
        public static implicit operator EntityId(Guid guid) => new(guid);
        public static explicit operator Guid(EntityId entityId) => entityId.AsGuid();

        // Desde int
        public static implicit operator EntityId(int value) => new((uint)value);
        public static explicit operator int(EntityId entityId) => (int)entityId.AsUInt();

        // Desde uint
        public static implicit operator EntityId(uint value) => new(value);
        public static explicit operator uint(EntityId entityId) => entityId.AsUInt();

        // Desde long
        public static implicit operator EntityId(long value) => new((ulong)value);
        public static explicit operator long(EntityId entityId) => (long)entityId.AsULong();

        // Desde ulong
        public static implicit operator EntityId(ulong value) => new(value);
        public static explicit operator ulong(EntityId entityId) => entityId.AsULong();

        // Desde string (validación incluida)
        public static implicit operator EntityId(string value) => new(ParseStringToBits(value));
        public static explicit operator string(EntityId entityId) => entityId.AsString();

        // Métodos auxiliares para conversión interna

        private Guid AsGuid()
        {
            if (_inner is Guid guid) return guid;

            // Convertir valores más pequeños en un Guid rellenando con ceros
            if (_inner is ulong ulongValue)
                return new Guid(0, 0, 0, BitConverter.GetBytes(ulongValue).Concat(new byte[8]).ToArray());
            if (_inner is uint uintValue)
                return new Guid(0, 0, 0, BitConverter.GetBytes(uintValue).Concat(new byte[12]).ToArray());

            throw new InvalidCastException("EntityId cannot be converted to Guid.");
        }

        private ulong AsULong()
        {
            if (_inner is ulong ulongValue) return ulongValue;

            // Convertir valores más pequeños rellenando con ceros
            if (_inner is uint uintValue) return uintValue;
            if (_inner is Guid guid) return BitConverter.ToUInt64(guid.ToByteArray(), 0);

            throw new InvalidCastException("EntityId cannot be converted to ulong.");
        }

        private uint AsUInt()
        {
            if (_inner is uint uintValue) return uintValue;

            // Convertir valores más pequeños rellenando con ceros
            if (_inner is ulong ulongValue) return (uint)(ulongValue & 0xFFFFFFFF);
            if (_inner is Guid guid) return BitConverter.ToUInt32(guid.ToByteArray(), 0);

            throw new InvalidCastException("EntityId cannot be converted to uint.");
        }

        private string AsString()
        {
            if (_inner is Guid guid) return guid.ToString();
            if (_inner is ulong ulongValue) return ulongValue.ToString(CultureInfo.InvariantCulture);
            if (_inner is uint uintValue) return uintValue.ToString(CultureInfo.InvariantCulture);

            throw new InvalidCastException("EntityId cannot be converted to string.");
        }

        // Validación y conversión de cadenas
        private static object ParseStringToBits(string value)
        {
            if (Guid.TryParse(value, out var guid)) return guid;

            if (ulong.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var ulongValue))
                return ulongValue;

            if (uint.TryParse(value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out var uintValue))
                return uintValue;

            throw new ArgumentException("String value is not a valid 128-bit representation.");
        }

        // Métodos de igualdad
        public override bool Equals(object obj) => obj is EntityId other && Equals(_inner, other._inner);
        public override int GetHashCode() => _inner?.GetHashCode() ?? 0;

        public static bool operator ==(EntityId left, EntityId right) => left.Equals(right);
        public static bool operator !=(EntityId left, EntityId right) => !left.Equals(right);

        // Representación como cadena
        public override string ToString() => AsString();
    }
}


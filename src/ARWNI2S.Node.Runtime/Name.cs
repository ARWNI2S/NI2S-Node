using System.Runtime.InteropServices;

namespace ARWNI2S.Runtime
{
    [StructLayout(LayoutKind.Explicit)]
    public readonly struct Name
    {
        public static readonly Name None = new();

        private const int MAX_NAME_CHARS = 128;
        private const StringComparison NAME_COMPARISON_TYPE = StringComparison.InvariantCultureIgnoreCase;

        [FieldOffset(0)]
        private readonly string _value;

        public Name() {  }

        public Name(string value)
        {
            if (value.Length > MAX_NAME_CHARS)
                throw new ArgumentOutOfRangeException($"{nameof(value)} exceeds {nameof(MAX_NAME_CHARS)} value of {MAX_NAME_CHARS}");

            _value = value;
        }

        public static implicit operator string(Name value) { return value._value; }
        public static implicit operator Name(string value) { return new Name(value); }

        public static bool operator ==(Name left, Name right) { return left._value.Equals(right._value, NAME_COMPARISON_TYPE); }
        public static bool operator !=(Name left, Name right) { return !(left == right); }

        public override bool Equals(object obj)
        {
            if (obj is not Name other)
                return false;

            return _value.Equals(other._value);
        }

        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }
    }
}
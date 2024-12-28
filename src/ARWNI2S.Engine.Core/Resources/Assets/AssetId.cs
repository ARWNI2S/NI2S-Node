using System.Runtime.InteropServices;

namespace ARWNI2S.Engine.Resources.Assets
{
    [StructLayout(LayoutKind.Explicit)]
    public struct AssetId
    {
        [FieldOffset(0)]
        private long _value;

        [FieldOffset(0)]
        private int _part1;

        [FieldOffset(4)]
        private int _part2;

        [FieldOffset(0)]
        private short _field0;

        [FieldOffset(2)]
        private short _field1;

        [FieldOffset(4)]
        private short _field2;

        [FieldOffset(6)]
        private short _field3;
    }
}
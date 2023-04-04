// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

namespace NI2S.Node.Network.Protocol
{
    public enum OpCode : byte
    {
        Connect = 1,
        Subscribe = 2,
        Publish = 3
    }

    public class NI2SPackage
    {
        public OpCode Code { get; set; }

        public short Sequence { get; set; }

        public string Body { get; set; }
    }
}

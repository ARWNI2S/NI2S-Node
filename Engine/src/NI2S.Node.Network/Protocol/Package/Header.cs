using System;
using System.Runtime.Serialization;

namespace NI2S.Node.Network.Protocol.Package
{
    [Serializable]
    public class Header
    {
        /// <summary>
        /// 
        /// </summary>
        public int PackageId { get; }

        /// <summary>
        /// 
        /// </summary>
        public DateTime DateTime { get; }

        /// <summary>
        /// 
        /// </summary>
        public Guid SenderId { get; }

        /// <summary>
        /// 
        /// </summary>
        public byte Type { get; }

    }
}
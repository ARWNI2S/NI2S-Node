// Copyrigth (c) 2023 Alternate Reality Worlds. Narrative Interactive Intelligent Simulator.

using NI2S.Node.Core;
using System;

namespace NI2S.Node.Engine
{
    public abstract class ModuleInfo : IDescriptor
    {
        public string Name { get; }

        public string SystemName { get; }

        protected ModuleInfo(string name, string systemName)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrEmpty(systemName)) throw new ArgumentNullException(nameof(systemName));

            Name = name.Trim();
            SystemName = systemName.Trim();
        }

        string IDescriptor.FriendlyName => Name;
    }
}

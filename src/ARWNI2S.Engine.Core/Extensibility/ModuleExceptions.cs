// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Extensibility
{
    [Serializable]
    internal class ModuleLoadException : NI2SException
    {
        public ModuleLoadException()
        {
        }

        public ModuleLoadException(string message) : base(message)
        {
        }

        public ModuleLoadException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
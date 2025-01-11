// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Configuration
{
    [AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
    public sealed class SecretAttribute : Attribute
    {
        private readonly string _name;

        public SecretAttribute(string name = null)
        {
            _name = name;
        }

        public string Name { get { return _name; } }
    }
}

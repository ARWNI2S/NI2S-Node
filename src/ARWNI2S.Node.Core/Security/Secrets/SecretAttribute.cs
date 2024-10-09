namespace ARWNI2S.Node.Core.Security.Secrets
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

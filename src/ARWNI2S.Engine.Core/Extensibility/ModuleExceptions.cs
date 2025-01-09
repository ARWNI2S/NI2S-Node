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
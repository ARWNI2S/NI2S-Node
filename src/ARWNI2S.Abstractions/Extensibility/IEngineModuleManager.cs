namespace ARWNI2S.Extensibility
{
    public interface IEngineModuleManager
    {
        IModuleCollection Modules { get; }

        void Register(IEngineModule module);
    }
}

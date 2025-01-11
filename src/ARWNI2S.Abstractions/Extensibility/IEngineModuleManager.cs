namespace ARWNI2S.Extensibility
{
    internal interface IEngineModuleManager
    {
        IModuleCollection Modules { get; }

        void Register(IEngineModule module, bool checkDependencies = true);
    }
}

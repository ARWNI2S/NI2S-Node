namespace ARWNI2S.Extensibility
{
    internal interface IModuleManager
    {
        IModuleCollection EngineModules { get; }
        IModuleCollection FrameworkModules { get; }
        IModuleCollection UserModules { get; }
        IModuleCollection NodeModules { get; }

        void Register(IModule module);
    }
}
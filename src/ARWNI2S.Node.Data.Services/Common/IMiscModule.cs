using ARWNI2S.Node.Data.Services.Plugins;

namespace ARWNI2S.Node.Data.Services.Common
{
    /// <summary>
    /// Misc module interface. 
    /// It's used by the modules that have a configuration page but don't fit any other category (such as payment or tax modules)
    /// </summary>
    public partial interface IMiscModule : IModule
    {
    }
}
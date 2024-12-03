using ARWNI2S.Extensibility;

namespace ARWNI2S
{
    public class ScenicModuleInfo : IModuleInfo
    {
        public readonly string SystemName = "Engine.Scene";
        public readonly string FriendlyName = "Scenic";

        string IDescriptor.SystemName { get => SystemName; set { } }
        string IDescriptor.FriendlyName { get => SystemName; set { } }
    }
}

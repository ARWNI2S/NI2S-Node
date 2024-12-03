using ARWNI2S.Extensibility;

namespace ARWNI2S
{
    public class NarrativeModuleInfo : IModuleInfo
    {
        public readonly string SystemName = "Engine.Narrative";
        public readonly string FriendlyName = "Narrator";

        string IDescriptor.SystemName { get => SystemName; set { } }
        string IDescriptor.FriendlyName { get => SystemName; set { } }
    }
}

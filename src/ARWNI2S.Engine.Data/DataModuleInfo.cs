using ARWNI2S.Extensibility;

namespace ARWNI2S.Data
{
    public class DataModuleInfo : IModuleInfo
    {
        public readonly string SystemName = "CoreData";
        public readonly string FriendlyName = "Data";

        string IDescriptor.SystemName { get => SystemName; set { } }
        string IDescriptor.FriendlyName { get => SystemName; set { } }
    }
}

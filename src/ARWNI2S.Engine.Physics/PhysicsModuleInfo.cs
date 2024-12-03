using ARWNI2S.Extensibility;

namespace ARWNI2S.Physics
{
    public class PhysicsModuleInfo : IModuleInfo
    {
        public readonly string SystemName = "CorePhysics";
        public readonly string FriendlyName = "Physics";

        string IDescriptor.SystemName { get => SystemName; set { } }
        string IDescriptor.FriendlyName { get => SystemName; set { } }
    }
}

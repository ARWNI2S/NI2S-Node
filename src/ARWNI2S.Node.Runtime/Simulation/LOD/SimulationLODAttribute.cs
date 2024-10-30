namespace ARWNI2S.Runtime.Simulation.LOD
{
    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class SimulationLODAttribute : Attribute
    {
        protected readonly int _levelOfDetail;

        public SimulationLODAttribute(int levelOfDetail) { _levelOfDetail = levelOfDetail; }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class SimulationLOD_0Attribute : SimulationLODAttribute
    {
        public SimulationLOD_0Attribute() : base(0) { }
    }

    [AttributeUsage(AttributeTargets.Method | AttributeTargets.Property, AllowMultiple = false)]
    public class SimulationLOD_MAXAttribute : SimulationLODAttribute
    {
        public SimulationLOD_MAXAttribute() : base(int.MaxValue) { }
    }
}

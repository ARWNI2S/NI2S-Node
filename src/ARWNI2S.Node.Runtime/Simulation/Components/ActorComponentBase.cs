using ARWNI2S.Node.Core.Entities;
using ARWNI2S.Runtime.Simulation.Objects;

namespace ARWNI2S.Runtime.Simulation.Components
{
    internal class ActorComponentBase : NI2SObject
    {
        private NI2SObject _parent;
        private int _componentId;

        public NI2SObject Parent => _parent;

        public override Guid UUID { get => _parent.UUID; }

        protected override object Id => _componentId;

        public override void BeginPlay()
        {

        }
    }

    internal class DataEntityComponent<TDataEntity> : NI2SObject<TDataEntity> where TDataEntity : DataEntity
    {
        public DataEntityComponent(TDataEntity dataEntity) : base(dataEntity)
        {
        }

        public override void BeginPlay()
        {

        }
    }
}

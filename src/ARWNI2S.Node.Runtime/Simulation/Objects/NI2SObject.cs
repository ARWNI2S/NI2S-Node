using ARWNI2S.Engine.Simulation;
using ARWNI2S.Infrastructure.Entities;
using ARWNI2S.Node.Core.Entities;

namespace ARWNI2S.Runtime.Simulation.Objects
{
    /// <summary>
    /// Represents a narrative interactive intelligent simulation abstract object
    /// </summary>
    public abstract class NI2SObject : SimulableBase, INI2SEntity
    {
        protected abstract object Id { get; }

        public DataEntity DataEntity { get; protected set; } = null;

        protected NI2SObject() { }

        public abstract void BeginPlay();

        object INI2SEntity.Id => Id;
    }

    /// <summary>
    /// Represents a narrative interactive intelligent simulation abstract object
    /// </summary>
    /// <typeparam name="TDataEntity"></typeparam>
    public abstract class NI2SObject<TDataEntity> : NI2SObject where TDataEntity : DataEntity
    {
        protected override object Id => DataEntity != null ? DataEntity.Id : UUID;

        public new TDataEntity DataEntity => (TDataEntity)base.DataEntity;

        protected NI2SObject(TDataEntity dataEntity) : base()
        {
            base.DataEntity = dataEntity;
        }
    }
}

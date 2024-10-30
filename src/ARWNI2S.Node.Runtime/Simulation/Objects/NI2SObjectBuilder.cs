using ARWNI2S.Engine.Simulation.Entities;

namespace ARWNI2S.Runtime.Simulation.Objects
{
    internal abstract class NI2SObjectBuilder<TNI2SObject> : IGameObjectBuilder<TNI2SObject> where TNI2SObject : NI2SObject
    {
        public abstract Guid Build(TNI2SObject gameObject);

        Guid IGameObjectBuilder.Build(IGameObject gameObject) => Build((TNI2SObject)gameObject);
    }
}

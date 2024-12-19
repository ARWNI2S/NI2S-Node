namespace ARWNI2S.Engine.Core
{
    public abstract class NI2SObject : BaseEntity, INiisObject
    {
        public virtual Guid UUID => (Guid)Id;
    }
}
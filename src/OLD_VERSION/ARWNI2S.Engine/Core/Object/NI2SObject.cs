using ARWNI2S.Engine.Runtime;

namespace ARWNI2S.Engine.Core.Object
{
    public abstract class NI2SObject : BaseEntity, INiisObject
    {
        private bool pendingKill;

        public virtual Guid UUID => (Guid)Id;

        protected NI2SObject() : this(new ObjectInitializer()) { }
        protected internal NI2SObject(ObjectInitializer objectInitializer) : base() { objectInitializer.Target = this; }

        protected internal bool IsValid(bool evenIfPendingKill, bool threadSafeTest = false)
        {
            if (pendingKill && !evenIfPendingKill)
                return false;

            return ValidateObject(threadSafeTest);
        }

        private bool ValidateObject(bool threadSafeTest = false)
        {
            return true;
        }

        protected static TObject NewObject<TObject>()
            where TObject : NI2SObject
        {
            return EngineContext.Current.Resolve<IObjectFactory<TObject>>()?.CreateInstance();
        }
    }
}
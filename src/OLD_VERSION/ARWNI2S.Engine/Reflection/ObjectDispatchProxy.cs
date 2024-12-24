using ARWNI2S.Engine.Core.Object;
using System.Reflection;

namespace ARWNI2S.Engine.Reflection
{
    internal class ObjectDispatchProxy<TObject> : DispatchProxy where TObject : class, INiisObject
    {
        public static TObject CreateProxy(TObject target)
        {
            var proxy = Create<TObject, ObjectDispatchProxy<TObject>>() as ObjectDispatchProxy<TObject>;
            proxy._target = target;
            return proxy as TObject;
        }

        private TObject _target;

        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            ArgumentNullException.ThrowIfNull(targetMethod);

            if (targetMethod.GetCustomAttribute<ForceInlineAttribute>() != null)
            {
                return targetMethod.Invoke(_target, args);
            }
            return null;
        }

    }
}

using ARWNI2S.Engine.Core;
using System.Reflection;

namespace ARWNI2S.Engine.Reflection
{
    internal class ActorDispatchProxy<TActor> : DispatchProxy where TActor : class, INiisActor
    {
        public static TActor CreateProxy(TActor target)
        {
            var proxy = Create<TActor, ActorDispatchProxy<TActor>>() as ActorDispatchProxy<TActor>;
            proxy._target = target;
            return proxy as TActor;
        }

        private TActor _target;

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

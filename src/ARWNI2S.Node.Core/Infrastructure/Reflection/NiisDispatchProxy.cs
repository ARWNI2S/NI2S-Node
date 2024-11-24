using System.Reflection;

namespace ARWNI2S.Node.Infrastructure.Reflection
{
    internal class NiisDispatchProxy : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            ArgumentNullException.ThrowIfNull(targetMethod);

            if (targetMethod.GetCustomAttribute<NI2SFunctionAttribute>(true) != null)
            {

            }

            if (targetMethod.ReturnType.IsAssignableFrom(typeof(Task)))
            {
                return null; 
            }
            else if(targetMethod.ReturnType.IsSpecialName)
                return null;
        }
    }
}

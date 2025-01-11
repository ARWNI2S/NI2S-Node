// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using System.Reflection;

namespace ARWNI2S.Engine.Simulation.Dispatcher
{
    internal class NI2SDispatchProxy : DispatchProxy
    {
        protected override object Invoke(MethodInfo targetMethod, object[] args)
        {
            throw new NotImplementedException();
        }
    }
}

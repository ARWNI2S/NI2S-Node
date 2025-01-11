// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

using ARWNI2S.Engine;

namespace ARWNI2S.Environment
{
    /// <summary>
    /// Provides support for lazy initialization
    /// </summary>
    /// <typeparam name="T">Specifies the type of element being lazily initialized.</typeparam>
    public partial class LazyInstance<T> : Lazy<T> where T : class
    {
        public LazyInstance()
            : base(() => EngineContext.Current.Resolve<T>())
        {

        }
    }
}
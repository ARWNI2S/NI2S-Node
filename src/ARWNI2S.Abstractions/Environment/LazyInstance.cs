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
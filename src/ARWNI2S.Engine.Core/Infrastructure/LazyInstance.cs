﻿namespace ARWNI2S.Infrastructure
{
    /// <summary>
    /// Provides support for lazy initialization
    /// </summary>
    /// <typeparam name="T">Specifies the type of element being lazily initialized.</typeparam>
    public partial class LazyInstance<T> : Lazy<T> where T : class
    {
        public LazyInstance()
            : base(() => NI2SContext.Current.Resolve<T>())
        {

        }
    }
}
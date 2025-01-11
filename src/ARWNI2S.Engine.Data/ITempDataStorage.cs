// This file is used by Code Analysis to maintain SuppressMessage
// attributes that are applied to this project.
// Project-level suppressions either have no target or are given
// a specific target and scoped to a namespace, type, member, etc.

namespace ARWNI2S.Engine.Data
{
    /// <summary>
    /// Represents temporary storage
    /// </summary>
    /// <typeparam name="T">Storage record mapping class</typeparam>
    public interface ITempDataStorage<T> : IQueryable<T>, IDisposable, IAsyncDisposable where T : class
    {
    }
}
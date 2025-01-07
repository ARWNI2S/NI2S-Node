using ARWNI2S.Data;
using ARWNI2S.Engine.Core;

namespace ARWNI2S.Engine.Resources
{
    /// <summary>
    /// Represents any resource entity
    /// </summary>
    public interface IResource
        : INiisEntity, IDisposable
    {
        ///// <summary>
        ///// Clear
        ///// </summary>
        //void Clear();
        ///// <summary>
        ///// Create
        ///// </summary>
        ///// <returns>Creation result as boolean.</returns>
        //bool Create();
        ///// <summary>
        ///// Destroy
        ///// </summary>
        //void Destroy();
        ///// <summary>
        ///// Refreate
        ///// </summary>
        ///// <returns>Recreation result as boolean.</returns>
        //bool Recreate();
        ///// <summary>
        ///// Returns resource size in bytes.
        ///// </summary>
        //int Size { get; }
        ///// <summary>
        ///// Returns disposed value.
        ///// </summary>
        //bool Disposed { get; }

        ///// <summary>
        ///// Gets or sets priority
        ///// </summary>
        //Priority Priority { get; }

        ///// <summary>
        ///// Gets or sets reference count
        ///// </summary>
        //int ReferenceCount { get; }

        ///// <summary>
        ///// Resource is locked
        ///// </summary>
        //bool Locked { get; }

        ///// <summary>
        ///// Gets or sets last access time
        ///// </summary>
        //TimeOnly LastAccess { get; }
    }

    /// <summary>
    /// Resource entity containing a inner data entity
    /// </summary>
    /// <typeparam name="TData"></typeparam>
    public interface IResource<TData> : IResource
        where TData : class, IDataEntity
    {
        /// <summary>
        /// Gets the resource's inner data entity
        /// </summary>
        TData Data { get; }
    }
}
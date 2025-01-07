namespace ARWNI2S.Engine.Resources
{

    /// <summary>
    /// Respresents a resource manager
    /// </summary>
    /// <typeparam name="TResource"></typeparam>
    public interface IResourceManager<TResource> //: IEnumerable<TResource>
        where TResource : IResource
    {
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="size"></param>
        ///// <returns></returns>
        //bool Reserve(int size);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <param name="resource"></param>
        ///// <returns></returns>
        //bool Insert(object id, TResource resource);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //bool Remove(object id);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="resource"></param>
        ///// <returns></returns>
        //bool Remove(TResource resource);

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //bool Destroy(object id);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="resource"></param>
        ///// <returns></returns>
        //bool Destroy(TResource resource);

        ////TResource this[object id] { get; }

        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //TResource Get(object id);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //TResource Lock(object id);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //int Unock(object id);
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //int Unock(TResource id);
    }
}

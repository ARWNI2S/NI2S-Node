namespace ARWNI2S.Engine.Memory
{
    /// <summary>
    /// IPoolSourceCreator
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPoolSourceCreator<T>
    {
        /// <summary>
        /// Creates the specified size.
        /// </summary>
        /// <param name="size">The size.</param>
        /// <param name="poolItems">The pool items.</param>
        /// <returns></returns>
        IPoolSource Create(int size, out T[] poolItems);
    }
}

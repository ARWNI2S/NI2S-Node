namespace ARWNI2S.Infrastructure.Collections.Internals
{
    /// <summary>
    /// The BinaryPredicate delegate type  encapsulates a method that takes two
    /// items of the same type, and returns a bool value representating 
    /// some relationship between them. For example, checking whether two
    /// items are equal or equivalent is one kind of binary predicate.
    /// </summary>
    /// <param name="item1">The first item.</param>
    /// <param name="item2">The second item.</param>
    /// <returns>Whether item1 and item2 satisfy the relationship that the BinaryPredicate defines.</returns>
    internal delegate bool BinaryPredicate<T>(T item1, T item2);

}

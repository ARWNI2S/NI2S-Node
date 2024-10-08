namespace ARWNI2S.Infrastructure.Abstractions.Collections.Generic
{
    /// <summary>
    /// Paged list interface
    /// </summary>
    public interface IPagedList<T> : IList<T>
    {
        /// <summary>
        /// Gets current page index
        /// </summary>
        int PageIndex { get; }
        /// <summary>
        /// Gets page size
        /// </summary>
        int PageSize { get; }
        /// <summary>
        /// Gets total elements count
        /// </summary>
        int TotalCount { get; }
        /// <summary>
        /// Gets total pages
        /// </summary>
        int TotalPages { get; }
        /// <summary>
        /// Gets if current page has previous page
        /// </summary>
        bool HasPreviousPage { get; }
        /// <summary>
        /// Gets if current page has next page
        /// </summary>
        bool HasNextPage { get; }
    }
}

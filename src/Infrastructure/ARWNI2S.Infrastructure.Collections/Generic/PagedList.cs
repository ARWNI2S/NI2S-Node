using ARWNI2S.Infrastructure.Abstractions.Collections.Generic;

namespace ARWNI2S.Infrastructure.Collections.Generic
{
    /// <summary>
    /// Paged list
    /// </summary>
    /// <typeparam name="T">T</typeparam>
    [Serializable]
    public class PagedList<T> : List<T>, IPagedList<T>
    {
        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IQueryable<T> source, int pageIndex, int pageSize)
        {
            int total = source.Count();
            TotalCount = total;
            TotalPages = total / pageSize;

            if (total % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        public PagedList(IList<T> source, int pageIndex, int pageSize)
        {
            TotalCount = source.Count();
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source.Skip(pageIndex * pageSize).Take(pageSize).ToList());
        }

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="source">source</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="totalCount">Total count</param>
        public PagedList(IEnumerable<T> source, int pageIndex, int pageSize, int totalCount)
        {
            TotalCount = totalCount;
            TotalPages = TotalCount / pageSize;

            if (TotalCount % pageSize > 0)
                TotalPages++;

            PageSize = pageSize;
            PageIndex = pageIndex;
            AddRange(source);
        }

        /// <summary>
        /// Gets current page index
        /// </summary>
        public int PageIndex { get; private set; }
        /// <summary>
        /// Gets page size
        /// </summary>
        public int PageSize { get; private set; }
        /// <summary>
        /// Gets total elements count
        /// </summary>
        public int TotalCount { get; private set; }
        /// <summary>
        /// Gets total pages
        /// </summary>
        public int TotalPages { get; private set; }

        /// <summary>
        /// Gets if current page has previous page
        /// </summary>
        public bool HasPreviousPage
        {
            get { return PageIndex > 0; }
        }
        /// <summary>
        /// Gets if current page has next page
        /// </summary>
        public bool HasNextPage
        {
            get { return PageIndex + 1 < TotalPages; }
        }
    }
}

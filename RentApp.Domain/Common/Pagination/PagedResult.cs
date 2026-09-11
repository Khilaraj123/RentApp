using System;
using System.Collections.Generic;
using System.Linq;

namespace RentApp.Domain.Common.Pagination
{
    public interface IPagedResult
    {
        /// <summary>Gets the current page number.</summary>
        int Page { get; }
        /// <summary>Gets the maximum number of items per page.</summary>
        int PageSize { get; }
        /// <summary>Gets the total number of items across all pages.</summary>
        int TotalItems { get; }
        /// <summary>Gets the total number of pages.</summary>
        int TotalPages { get; }
        /// <summary>Gets a value indicating whether there is a previous page.</summary>
        bool HasPrevious { get; }
        /// <summary>Gets a value indicating whether there is a next page.</summary>
        bool HasNext { get; }
    }

    public class PagedResult<T> : IPagedResult
    {
        public IReadOnlyList<T> Items { get; init; } = [];

        public int Page { get; init; }
        public int PageSize { get; init; }
        public int TotalItems { get; init; }
        public int TotalPages { get; init; }

        public bool HasPrevious => Page > 1;
        public bool HasNext => Page < TotalPages;

        public PagedResult<TTarget> Map<TTarget>(Func<T, TTarget> mapper)
        {
            ArgumentNullException.ThrowIfNull(mapper);

            return new PagedResult<TTarget>
            {
                Items = Items.Select(mapper).ToList(),
                Page = Page,
                PageSize = PageSize,
                TotalItems = TotalItems,
                TotalPages = TotalPages
            };
        }
    }
}

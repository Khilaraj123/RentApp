namespace RentApp.Application.Common.Pagination.Offset
{
    public interface IPagedResult
    {
        int Page { get; }
        int PageSize { get; }
        int TotalItems { get; }
        int TotalPages { get; }
        bool HasPrevious { get; }
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

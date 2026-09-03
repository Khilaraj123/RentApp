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

    public class PagedResult
    {
    }
}

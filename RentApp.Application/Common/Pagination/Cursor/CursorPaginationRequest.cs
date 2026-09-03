namespace RentApp.Application.Common.Pagination.Cursor
{
    public sealed class CursorPaginationRequest
    {
        private const int MaxPageSize = 100;
        private int _pageSize = 20;

        public string? Cursor { get; init; }
        public int PageSize
        {
            get => _pageSize;
            init => _pageSize = value switch
            {
                < 1 => 1,
                > MaxPageSize => MaxPageSize,
                _ => value
            };
        }
    }
}

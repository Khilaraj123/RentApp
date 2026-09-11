namespace RentApp.Domain.Common.Pagination
{
    public sealed class PaginationRequest
    {
        private const int MaxPageSize = 100;

        private int _page = 1;
        private int _pageSize = 20;

        /// <summary>
        /// Gets the page number requested (1-based index).
        /// </summary>
        public int Page
        {
            get => _page;
            init => _page = value < 1 ? 1 : value;
        }

        /// <summary>
        /// Gets the maximum number of items to return per page.
        /// </summary>
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

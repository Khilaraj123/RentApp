namespace RentApp.Application.Common.Pagination.Cursor
{
    public sealed class CursorPagedResult<T>
    {
        public IReadOnlyList<T> Items { get; init; } = [];
        public string? NextCursor { get; init; }
        public bool HasNext { get; init; }

        public CursorPagedResult<TTarget> Map<TTarget>(Func<T, TTarget> mapper)
        {
            ArgumentNullException.ThrowIfNull(mapper);

            return new CursorPagedResult<TTarget>
            {
                Items = Items.Select(mapper).ToList(),
                NextCursor = NextCursor,
                HasNext = HasNext
            };
        }
    }
}

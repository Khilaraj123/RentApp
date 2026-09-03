using System.Reflection.Metadata;

namespace RentApp.Application.Common.Pagination.Cursor
{
    public interface ICursorTokenService
    {
        string Encode<T>(T value);
        string Decode<T>(string cursor);
    }
}

namespace RentApp.Application.Common.Exceptions
{
    public class NotFoundException : AppException
    {
        public NotFoundException(string entityName, object key)
            : base($"{entityName} '{key}' was not found.") { }
    }
}

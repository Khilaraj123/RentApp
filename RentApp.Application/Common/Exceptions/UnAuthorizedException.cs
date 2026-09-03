namespace RentApp.Application.Common.Exceptions
{
    public class UnAuthorizedException : AppException
    {
        public UnAuthorizedException(string message) 
            : base(message) { }
    }
}

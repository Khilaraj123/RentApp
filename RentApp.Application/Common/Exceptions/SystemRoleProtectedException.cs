namespace RentApp.Application.Common.Exceptions
{
    public class SystemRoleProtectedException : AppException
    {
        public SystemRoleProtectedException() :
            base("Cannot modify or delete the default system role.") { }
    }
}

namespace RentApp.Application.DTOs.Users.Auth
{
    public class RefreshTokenResult
    {
        public Guid Id { get; init; }

        public string Token { get; init; } = string.Empty;

        public Guid TokenFamilyId { get; init; }

        public DateTime ExpiresAt { get; init; }
    }
}

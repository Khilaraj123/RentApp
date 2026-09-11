namespace RentApp.Application.DTOs.Users.Auth;

public class AccessTokenResult
{
    public string Token { get; init; } = string.Empty;
    public DateTime ExpiresAt { get; init; }
}

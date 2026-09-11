namespace RentApp.Application.DTOs.Users.Auth;

public record LoginRequestDto
{
    public string EmailOrPhone { get; init; } = string.Empty;
    public string Password { get; init; } = string.Empty;

    //Honey pot
    public string? Website {  get; init; } = string.Empty;
}

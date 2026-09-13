namespace RentApp.Presentation.ViewModels;

public class NavbarViewModel
{
    public bool IsAuthenticated { get; set; }
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Initials { get; set; }
    public string? Role { get; set; }
}

using System.Collections.Generic;

namespace RentApp.Presentation.ViewModels;

public class SidebarViewModel
{
    public string Role { get; set; } = "User";
    public string RoleDisplayName { get; set; } = "User Portal";
    public string RoleTheme { get; set; } = "theme-user";
    public string? FullName { get; set; }
    public string? Email { get; set; }
    public string? ProfilePictureUrl { get; set; }
    public string? Initials { get; set; }
    public bool IsAuthenticated { get; set; }
    public List<SidebarNavGroup> Groups { get; set; } = new();
}

public class SidebarNavGroup
{
    public string? Title { get; set; }
    public List<SidebarNavItem> Items { get; set; } = new();
}

public class SidebarNavItem
{
    public string Title { get; set; } = string.Empty;
    public string? Controller { get; set; }
    public string? Action { get; set; }
    public string? Area { get; set; }
    public string? Url { get; set; }
    public string IconSvg { get; set; } = string.Empty;
    public string? Badge { get; set; }
    public string? BadgeClass { get; set; }
    public bool IsActive { get; set; }
    public List<SidebarNavItem>? SubItems { get; set; }
}

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentApp.Domain.Constants;
using RentApp.Domain.Entities.Users;
using RentApp.Presentation.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RentApp.Presentation.ViewComponents;

public class SidebarViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;

    public SidebarViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync(string? role = null)
    {
        var model = new SidebarViewModel
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false
        };

        // Determine user details if authenticated
        if (model.IsAuthenticated && UserClaimsPrincipal != null)
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            if (user != null)
            {
                model.FullName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : user.UserName;
                model.Email = user.Email;
                model.ProfilePictureUrl = user.ProfilePictureUrl;

                var initials = "";
                if (!string.IsNullOrWhiteSpace(user.FirstName)) initials += user.FirstName[0];
                if (!string.IsNullOrWhiteSpace(user.LastName)) initials += user.LastName[0];
                else if (string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.Email)) initials = user.Email[0].ToString();

                model.Initials = !string.IsNullOrEmpty(initials) ? initials.ToUpperInvariant() : "U";

                if (string.IsNullOrWhiteSpace(role))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    if (userRoles.Contains(Roles.Admin)) role = Roles.Admin;
                    else if (userRoles.Contains(Roles.Owner)) role = Roles.Owner;
                    else if (userRoles.Contains(Roles.Renter)) role = Roles.Renter;
                    else role = userRoles.FirstOrDefault() ?? Roles.User;
                }
            }
        }

        // Fallback role resolution if not resolved yet
        if (string.IsNullOrWhiteSpace(role))
        {
            if (User.IsInRole(Roles.Admin)) role = Roles.Admin;
            else if (User.IsInRole(Roles.Owner)) role = Roles.Owner;
            else if (User.IsInRole(Roles.Renter)) role = Roles.Renter;
            else role = Roles.Owner; // Default preview
        }

        model.Role = role;

        // Current route info for matching active link
        var currentArea = ViewContext.RouteData.Values["area"]?.ToString() ?? "";
        var currentController = ViewContext.RouteData.Values["controller"]?.ToString() ?? "";
        var currentAction = ViewContext.RouteData.Values["action"]?.ToString() ?? "";
        var currentPath = ViewContext.HttpContext.Request.Path.Value ?? "";

        // Build navigation structure based on role
        if (string.Equals(role, Roles.Admin, StringComparison.OrdinalIgnoreCase))
        {
            model.RoleDisplayName = "Admin Console";
            model.RoleTheme = "theme-admin";
            model.Groups = BuildAdminNavigation(currentArea, currentController, currentAction, currentPath);
        }
        else if (string.Equals(role, Roles.Renter, StringComparison.OrdinalIgnoreCase))
        {
            model.RoleDisplayName = "Renter Portal";
            model.RoleTheme = "theme-renter";
            model.Groups = BuildRenterNavigation(currentArea, currentController, currentAction, currentPath);
        }
        else
        {
            // Owner / Host by default
            model.RoleDisplayName = "Owner Portal";
            model.RoleTheme = "theme-owner";
            model.Groups = BuildOwnerNavigation(currentArea, currentController, currentAction, currentPath);
        }

        return View(model);
    }

    private List<SidebarNavGroup> BuildAdminNavigation(string area, string controller, string action, string path)
    {
        return new List<SidebarNavGroup>
        {
            new SidebarNavGroup
            {
                Title = "OVERVIEW",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Admin Dashboard",
                        Area = "Admins",
                        Controller = "Home",
                        Action = "Index",
                        IconSvg = "<path d='m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z'/><polyline points='9 22 9 12 15 12 15 22'/>",
                        IsActive = (area.Equals("Admins", StringComparison.OrdinalIgnoreCase) && controller.Equals("Home", StringComparison.OrdinalIgnoreCase)) || path.Contains("/Admins", StringComparison.OrdinalIgnoreCase)
                    }
                }
            },
            new SidebarNavGroup
            {
                Title = "PLATFORM MANAGEMENT",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Users & Roles",
                        Area = "Admins",
                        Controller = "Users",
                        Action = "Index",
                        IconSvg = "<path d='M16 21v-2a4 4 0 0 0-4-4H6a4 4 0 0 0-4 4v2'/><circle cx='9' cy='7' r='4'/><path d='M22 21v-2a4 4 0 0 0-3-3.87'/><path d='M16 3.13a4 4 0 0 1 0 7.75'/>",
                        Badge = "Verified",
                        BadgeClass = "badge-role-admin",
                        IsActive = controller.Equals("Users", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Property Listings",
                        Area = "Admins",
                        Controller = "Listings",
                        Action = "Index",
                        IconSvg = "<rect width='16' height='20' x='4' y='2' rx='2' ry='2'/><path d='M9 22v-4h6v4'/><path d='M8 6h.01'/><path d='M16 6h.01'/><path d='M8 10h.01'/><path d='M16 10h.01'/><path d='M8 14h.01'/><path d='M16 14h.01'/>",
                        Badge = "Approval",
                        BadgeClass = "badge-warning-custom",
                        IsActive = controller.Equals("Listings", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Categories & Amenities",
                        Area = "Admins",
                        Controller = "Categories",
                        Action = "Index",
                        IconSvg = "<path d='m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z'/><path d='M20.59 13.41l-7.17 7.17a2 2 0 0 1-2.83 0L2 12V2h10l8.59 8.59a2 2 0 0 1 0 2.82z'/><line x1='7' y1='7' x2='7.01' y2='7'/>",
                        IsActive = controller.Equals("Categories", StringComparison.OrdinalIgnoreCase)
                    }
                }
            },
            new SidebarNavGroup
            {
                Title = "OPERATIONS & COMPLIANCE",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "All Bookings",
                        Area = "Admins",
                        Controller = "Bookings",
                        Action = "Index",
                        IconSvg = "<rect width='18' height='18' x='3' y='4' rx='2'/><line x1='16' x2='16' y1='2' y2='6'/><line x1='8' x2='8' y1='2' y2='6'/><line x1='3' x2='21' y1='10' y2='10'/><path d='m9 16 2 2 4-4'/>",
                        IsActive = controller.Equals("Bookings", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Payments & Payouts",
                        Area = "Admins",
                        Controller = "Payments",
                        Action = "Index",
                        IconSvg = "<rect width='20' height='14' x='2' y='5' rx='2'/><line x1='2' x2='22' y1='10' y2='10'/>",
                        IsActive = controller.Equals("Payments", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Reviews & Moderation",
                        Area = "Admins",
                        Controller = "Reviews",
                        Action = "Index",
                        IconSvg = "<path d='M12 22s8-4 8-10V5l-8-3-8 3v7c0 6 8 10 8 10z'/>",
                        IsActive = controller.Equals("Reviews", StringComparison.OrdinalIgnoreCase)
                    }
                }
            },
            new SidebarNavGroup
            {
                Title = "SYSTEM",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Platform Analytics",
                        Area = "Admins",
                        Controller = "Analytics",
                        Action = "Index",
                        IconSvg = "<path d='M3 3v18h18'/><path d='m19 9-5 5-4-4-3 3'/>",
                        IsActive = controller.Equals("Analytics", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "System Settings",
                        Area = "Admins",
                        Controller = "Settings",
                        Action = "Index",
                        IconSvg = "<circle cx='12' cy='12' r='3'/><path d='M19.4 15a1.65 1.65 0 0 0 .33 1.82l.06.06a2 2 0 0 1 0 2.83 2 2 0 0 1-2.83 0l-.06-.06a1.65 1.65 0 0 0-1.82-.33 1.65 1.65 0 0 0-1 1.51V21a2 2 0 0 1-2 2 2 2 0 0 1-2-2v-.09A1.65 1.65 0 0 0 9 19.4a1.65 1.65 0 0 0-1.82.33l-.06.06a2 2 0 0 1-2.83 0 2 2 0 0 1 0-2.83l.06-.06a1.65 1.65 0 0 0 .33-1.82 1.65 1.65 0 0 0-1.51-1H3a2 2 0 0 1-2-2 2 2 0 0 1 2-2h.09A1.65 1.65 0 0 0 4.6 9a1.65 1.65 0 0 0-.33-1.82l-.06-.06a2 2 0 0 1 0-2.83 2 2 0 0 1 2.83 0l.06.06a1.65 1.65 0 0 0 1.82.33H9a1.65 1.65 0 0 0 1-1.51V3a2 2 0 0 1 2-2 2 2 0 0 1 2 2v.09a1.65 1.65 0 0 0 1 1.51 1.65 1.65 0 0 0 1.82-.33l.06-.06a2 2 0 0 1 2.83 0 2 2 0 0 1 0 2.83l-.06.06a1.65 1.65 0 0 0-.33 1.82V9a1.65 1.65 0 0 0 1.51 1H21a2 2 0 0 1 2 2 2 2 0 0 1-2 2h-.09a1.65 1.65 0 0 0-1.51 1z'/>",
                        IsActive = controller.Equals("Settings", StringComparison.OrdinalIgnoreCase)
                    }
                }
            }
        };
    }

    private List<SidebarNavGroup> BuildOwnerNavigation(string area, string controller, string action, string path)
    {
        return new List<SidebarNavGroup>
        {
            new SidebarNavGroup
            {
                Title = "OVERVIEW",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Host Dashboard",
                        Controller = "Dashboard",
                        Action = "Index",
                        IconSvg = "<path d='m3 9 9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z'/><polyline points='9 22 9 12 15 12 15 22'/>",
                        IsActive = controller.Equals("Dashboard", StringComparison.OrdinalIgnoreCase) && string.IsNullOrEmpty(area)
                    }
                }
            },
            new SidebarNavGroup
            {
                Title = "MY PROPERTIES",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "My Listings",
                        Controller = "Listing",
                        Action = "Index",
                        IconSvg = "<rect width='16' height='20' x='4' y='2' rx='2' ry='2'/><path d='M9 22v-4h6v4'/><path d='M8 6h.01'/><path d='M16 6h.01'/><path d='M8 10h.01'/><path d='M16 10h.01'/><path d='M8 14h.01'/><path d='M16 14h.01'/>",
                        Badge = "Active",
                        BadgeClass = "badge-role-owner",
                        IsActive = controller.Equals("Listing", StringComparison.OrdinalIgnoreCase) && action.Equals("Index", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Add New Listing",
                        Controller = "Listing",
                        Action = "Create",
                        IconSvg = "<circle cx='12' cy='12' r='10'/><line x1='12' y1='8' x2='12' y2='16'/><line x1='8' y1='12' x2='16' y2='12'/>",
                        Badge = "+ New",
                        BadgeClass = "badge-success-custom",
                        IsActive = controller.Equals("Listing", StringComparison.OrdinalIgnoreCase) && action.Equals("Create", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Calendar & Availability",
                        Controller = "Listing",
                        Action = "Calendar",
                        IconSvg = "<rect width='18' height='18' x='3' y='4' rx='2'/><line x1='16' x2='16' y1='2' y2='6'/><line x1='8' x2='8' y1='2' y2='6'/><line x1='3' x2='21' y1='10' y2='10'/>",
                        IsActive = controller.Equals("Listing", StringComparison.OrdinalIgnoreCase) && action.Equals("Calendar", StringComparison.OrdinalIgnoreCase)
                    }
                }
            },
            new SidebarNavGroup
            {
                Title = "RENTALS & GUESTS",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Bookings & Requests",
                        Controller = "Booking",
                        Action = "Index",
                        IconSvg = "<path d='m9 11 3 3L22 4'/><path d='M21 12v7a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2h11'/>",
                        Badge = "3 New",
                        BadgeClass = "badge-primary-custom",
                        IsActive = controller.Equals("Booking", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Guest Reviews",
                        Controller = "Review",
                        Action = "Index",
                        IconSvg = "<polygon points='12 2 15.09 8.26 22 9.27 17 14.14 18.18 21.02 12 17.77 5.82 21.02 7 14.14 2 9.27 8.91 8.26 12 2'/>",
                        IsActive = controller.Equals("Review", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Inquiries & Messages",
                        Controller = "Message",
                        Action = "Index",
                        IconSvg = "<path d='M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z'/>",
                        Badge = "5",
                        BadgeClass = "badge-info-custom",
                        IsActive = controller.Equals("Message", StringComparison.OrdinalIgnoreCase)
                    }
                }
            },
            new SidebarNavGroup
            {
                Title = "FINANCES & EARNINGS",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Earnings & Payouts",
                        Controller = "Payment",
                        Action = "Index",
                        IconSvg = "<line x1='12' y1='2' x2='12' y2='22'/><path d='M17 5H9.5a3.5 3.5 0 0 0 0 7h5a3.5 3.5 0 0 1 0 7H6'/>",
                        IsActive = controller.Equals("Payment", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Host Profile & Settings",
                        Controller = "Account",
                        Action = "Profile",
                        IconSvg = "<circle cx='12' cy='8' r='5'/><path d='M20 21a8 8 0 0 0-16 0'/>",
                        IsActive = controller.Equals("Account", StringComparison.OrdinalIgnoreCase)
                    }
                }
            }
        };
    }

    private List<SidebarNavGroup> BuildRenterNavigation(string area, string controller, string action, string path)
    {
        return new List<SidebarNavGroup>
        {
            new SidebarNavGroup
            {
                Title = "EXPLORE RENTALS",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Browse Properties",
                        Controller = "Home",
                        Action = "Index",
                        IconSvg = "<circle cx='11' cy='11' r='8'/><line x1='21' y1='21' x2='16.65' y2='16.65'/>",
                        IsActive = controller.Equals("Home", StringComparison.OrdinalIgnoreCase) && action.Equals("Index", StringComparison.OrdinalIgnoreCase) && path == "/"
                    },
                    new SidebarNavItem
                    {
                        Title = "Near You",
                        Controller = "Home",
                        Action = "NearYou",
                        IconSvg = "<path d='M20 10c0 4.993-5.539 10.193-7.399 11.799a1 1 0 0 1-1.202 0C9.539 20.193 4 14.993 4 10a8 8 0 0 1 16 0'/><circle cx='12' cy='10' r='3'/>",
                        IsActive = path.Contains("NearYou", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Saved Wishlists",
                        Controller = "Wishlist",
                        Action = "Index",
                        IconSvg = "<path d='M19 14c1.49-1.46 3-3.21 3-5.5A5.5 5.5 0 0 0 16.5 3c-1.76 0-3 .5-4.5 2-1.5-1.5-2.74-2-4.5-2A5.5 5.5 0 0 0 2 8.5c0 2.3 1.5 4.05 3 5.5l7 7Z'/>",
                        IsActive = controller.Equals("Wishlist", StringComparison.OrdinalIgnoreCase)
                    }
                }
            },
            new SidebarNavGroup
            {
                Title = "MY RENTALS & ACTIVITY",
                Items = new List<SidebarNavItem>
                {
                    new SidebarNavItem
                    {
                        Title = "Current Bookings",
                        Controller = "Booking",
                        Action = "Index",
                        IconSvg = "<rect width='18' height='18' x='3' y='4' rx='2'/><line x1='16' x2='16' y1='2' y2='6'/><line x1='8' x2='8' y1='2' y2='6'/><line x1='3' x2='21' y1='10' y2='10'/><path d='m9 16 2 2 4-4'/>",
                        IsActive = controller.Equals("Booking", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Messages & Chats",
                        Controller = "Message",
                        Action = "Index",
                        IconSvg = "<path d='M21 15a2 2 0 0 1-2 2H7l-4 4V5a2 2 0 0 1 2-2h14a2 2 0 0 1 2 2z'/>",
                        IsActive = controller.Equals("Message", StringComparison.OrdinalIgnoreCase)
                    },
                    new SidebarNavItem
                    {
                        Title = "Account Profile",
                        Controller = "Account",
                        Action = "Profile",
                        IconSvg = "<circle cx='12' cy='8' r='5'/><path d='M20 21a8 8 0 0 0-16 0'/>",
                        IsActive = controller.Equals("Account", StringComparison.OrdinalIgnoreCase)
                    }
                }
            }
        };
    }
}

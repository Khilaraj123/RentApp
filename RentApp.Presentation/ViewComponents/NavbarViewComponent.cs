using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RentApp.Domain.Entities.Users;
using RentApp.Presentation.ViewModels;
using System.Linq;
using System.Threading.Tasks;

namespace RentApp.Presentation.ViewComponents;

public class NavbarViewComponent : ViewComponent
{
    private readonly UserManager<ApplicationUser> _userManager;

    public NavbarViewComponent(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var model = new NavbarViewModel
        {
            IsAuthenticated = User.Identity?.IsAuthenticated ?? false
        };

        if (model.IsAuthenticated && UserClaimsPrincipal != null)
        {
            var user = await _userManager.GetUserAsync(UserClaimsPrincipal);
            if (user != null)
            {
                model.FullName = !string.IsNullOrWhiteSpace(user.FullName) ? user.FullName : user.UserName;
                model.Email = user.Email;
                model.ProfilePictureUrl = user.ProfilePictureUrl;

                var roles = await _userManager.GetRolesAsync(user);
                model.Role = roles.FirstOrDefault();

                var initials = "";
                if (!string.IsNullOrWhiteSpace(user.FirstName))
                {
                    initials += user.FirstName[0];
                }
                if (!string.IsNullOrWhiteSpace(user.LastName))
                {
                    initials += user.LastName[0];
                }
                else if (string.IsNullOrWhiteSpace(user.FirstName) && !string.IsNullOrWhiteSpace(user.Email))
                {
                    initials = user.Email[0].ToString();
                }

                model.Initials = !string.IsNullOrEmpty(initials) ? initials.ToUpperInvariant() : "U";
            }
            else
            {
                var name = UserClaimsPrincipal.Identity?.Name ?? "User";
                model.FullName = name;
                model.Initials = name.Length > 0 ? name[0].ToString().ToUpperInvariant() : "U";
            }
        }

        return View(model);
    }
}

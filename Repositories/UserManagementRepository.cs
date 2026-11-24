using Deliverable_2_StudentBookExchangeMVC.Models;
using Microsoft.AspNetCore.Identity;

namespace Deliverable_2_StudentBookExchangeMVC.Repositories
{
    public class UserManagementRepository : IUserManagementRepository
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public UserManagementRepository(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        public async Task<IList<AdminUserViewModel>> GetAllAsync()
        {
            var result = new List<AdminUserViewModel>();
            foreach (var user in _userManager.Users.OrderBy(u => u.Email))
            {
                result.Add(new AdminUserViewModel
                {
                    Id = user.Id,
                    Email = user.Email,
                    EmailConfirmed = user.EmailConfirmed,
                    Roles = await _userManager.GetRolesAsync(user)
                });
            }
            return result;
        }

        public async Task<bool> SetRoleAsync(string userId, string role)
        {
            if (role is not ("Student" or "Admin") || !await _roleManager.RoleExistsAsync(role))
                return false;

            var user = await _userManager.FindByIdAsync(userId);
            if (user == null) return false;

            var currentRoles = await _userManager.GetRolesAsync(user);
            if (currentRoles.Count > 0)
                await _userManager.RemoveFromRolesAsync(user, currentRoles);

            var result = await _userManager.AddToRoleAsync(user, role);
            return result.Succeeded;
        }
    }
}

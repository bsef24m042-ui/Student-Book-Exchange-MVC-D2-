using Deliverable_2_StudentBookExchangeMVC.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Deliverable_2_StudentBookExchangeMVC.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IDashboardRepository _dashboard;
        private readonly IUserManagementRepository _users;

        public AdminController(
            IDashboardRepository dashboard,
            IUserManagementRepository users)
        {
            _dashboard = dashboard;
            _users = users;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _dashboard.GetSummaryAsync());
        }

        public async Task<IActionResult> Users()
        {
            return View(await _users.GetAllAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetRole(string userId, string role)
        {
            var success = await _users.SetRoleAsync(userId, role);
            TempData[success ? "Success" : "Error"] = success
                ? "User role updated."
                : "Could not update user role.";
            return RedirectToAction(nameof(Users));
        }
    }
}

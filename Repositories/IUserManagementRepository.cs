using Deliverable_2_StudentBookExchangeMVC.Models;

namespace Deliverable_2_StudentBookExchangeMVC.Repositories
{
    public interface IUserManagementRepository
    {
        Task<IList<AdminUserViewModel>> GetAllAsync();
        Task<bool> SetRoleAsync(string userId, string role);
    }
}

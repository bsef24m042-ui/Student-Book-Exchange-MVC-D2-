using Deliverable_2_StudentBookExchangeMVC.Models;

namespace Deliverable_2_StudentBookExchangeMVC.Repositories
{
    public interface IDashboardRepository
    {
        Task<DashboardViewModel> GetSummaryAsync();
    }
}

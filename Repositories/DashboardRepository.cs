using Dapper;
using Deliverable_2_StudentBookExchangeMVC.Data;
using Deliverable_2_StudentBookExchangeMVC.Models;

namespace Deliverable_2_StudentBookExchangeMVC.Repositories
{
    public class DashboardRepository : IDashboardRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public DashboardRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<DashboardViewModel> GetSummaryAsync()
        {
            const string sql = @"
                SELECT
                    (SELECT COUNT(*) FROM Books) AS TotalBooks,
                    (SELECT COUNT(*) FROM Books WHERE IsAvailable = 1) AS AvailableBooks,
                    (SELECT COUNT(*) FROM AspNetUsers) AS TotalUsers;";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleAsync<DashboardViewModel>(sql);
        }
    }
}

using Microsoft.Data.SqlClient;

namespace Deliverable_2_StudentBookExchangeMVC.Data
{
    public interface ISqlConnectionFactory
    {
        SqlConnection CreateConnection();
    }

    public class SqlConnectionFactory : ISqlConnectionFactory
    {
        private readonly string _connectionString;

        public SqlConnectionFactory(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection")
                ?? throw new InvalidOperationException("DefaultConnection is missing.");
        }

        public SqlConnection CreateConnection() => new SqlConnection(_connectionString);
    }
}

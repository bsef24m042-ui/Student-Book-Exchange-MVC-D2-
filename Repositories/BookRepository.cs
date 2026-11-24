using Dapper;
using Deliverable_2_StudentBookExchangeMVC.Data;
using Deliverable_2_StudentBookExchangeMVC.Models;

namespace Deliverable_2_StudentBookExchangeMVC.Repositories
{
    public class BookRepository : IBookRepository
    {
        private readonly ISqlConnectionFactory _connectionFactory;

        public BookRepository(ISqlConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            const string sql = @"SELECT Id, Title, Author, Subject, Price, Condition,
                Description, IsAvailable, SellerId FROM Books ORDER BY Id DESC";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Book>(sql);
        }

        public async Task<IEnumerable<Book>> SearchAsync(string? term)
        {
            const string sql = @"SELECT Id, Title, Author, Subject, Price, Condition,
                Description, IsAvailable, SellerId FROM Books
                WHERE (@Term IS NULL OR @Term = ''
                OR Title LIKE '%' + @Term + '%'
                OR Author LIKE '%' + @Term + '%'
                OR Subject LIKE '%' + @Term + '%')
                ORDER BY Id DESC";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Book>(sql, new { Term = term?.Trim() });
        }

        public async Task<Book?> GetByIdAsync(int id)
        {
            const string sql = @"SELECT Id, Title, Author, Subject, Price, Condition,
                Description, IsAvailable, SellerId FROM Books WHERE Id = @Id";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QuerySingleOrDefaultAsync<Book>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Book book)
        {
            const string sql = @"INSERT INTO Books
                (Title, Author, Subject, Price, Condition, Description, IsAvailable, SellerId)
                VALUES (@Title, @Author, @Subject, @Price, @Condition, @Description, @IsAvailable, @SellerId);
                SELECT CAST(SCOPE_IDENTITY() AS int);";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<int>(sql, book);
        }

        public async Task UpdateAsync(Book book)
        {
            const string sql = @"UPDATE Books SET Title=@Title, Author=@Author, Subject=@Subject,
                Price=@Price, Condition=@Condition, Description=@Description,
                IsAvailable=@IsAvailable, SellerId=@SellerId WHERE Id=@Id";
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync(sql, book);
        }

        public async Task DeleteAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();
            await connection.ExecuteAsync("DELETE FROM Books WHERE Id=@Id", new { Id = id });
        }

        public async Task<IEnumerable<Book>> GetBySellerAsync(string sellerId)
        {
            const string sql = @"SELECT Id, Title, Author, Subject, Price, Condition,
                Description, IsAvailable, SellerId FROM Books WHERE SellerId=@SellerId ORDER BY Id DESC";
            using var connection = _connectionFactory.CreateConnection();
            return await connection.QueryAsync<Book>(sql, new { SellerId = sellerId });
        }
    }
}

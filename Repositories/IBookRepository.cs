using Deliverable_2_StudentBookExchangeMVC.Models;

namespace Deliverable_2_StudentBookExchangeMVC.Repositories
{
    public interface IBookRepository
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<IEnumerable<Book>> SearchAsync(string? term);
        Task<Book?> GetByIdAsync(int id);
        Task<int> AddAsync(Book book);
        Task UpdateAsync(Book book);
        Task DeleteAsync(int id);
        Task<IEnumerable<Book>> GetBySellerAsync(string sellerId);
    }
}

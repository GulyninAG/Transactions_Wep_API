using Transactions_Web_API.Domain;

namespace Transactions_Web_API.Infrastructure.Interfaces
{
    public interface ITransactionRepository
    {
        Task<List<Transaction>> GetAllAsync();
        Task<Transaction> GetByIdAsync(Guid id);
        Task AddAsync(Transaction transaction);
        Task DeleteAsync(Guid id);
    }
}

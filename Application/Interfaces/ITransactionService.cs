using Transactions_Web_API.Domain;

namespace Transactions_Web_API.Application.Interfaces
{
    public interface ITransactionService
    {
        Task<List<Transaction>> GetTransactionsAsync();
        Task<Transaction> GetTransactionByIdAsync(Guid id);
        Task AddTransactionAsync(Transaction transaction);
        Task DeleteTransactionAsync(Guid id);
    }
}

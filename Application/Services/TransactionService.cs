using Transactions_Web_API.Application.Interfaces;
using Transactions_Web_API.Domain;
using Transactions_Web_API.Infrastructure.Interfaces;

namespace Transactions_Web_API.Application.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _transactionRepository;

        public TransactionService(ITransactionRepository transactionRepository)
        {
            _transactionRepository = transactionRepository;
        }

        public async Task<List<Transaction>> GetTransactionsAsync()
        {
            return await _transactionRepository.GetAllAsync();
        }

        public async Task<Transaction> GetTransactionByIdAsync(Guid id)
        {
            return await _transactionRepository.GetByIdAsync(id);
        }

        public async Task AddTransactionAsync(Transaction transaction)
        {
            await _transactionRepository.AddAsync(transaction);
        }

        public async Task DeleteTransactionAsync(Guid id)
        {
            await _transactionRepository.DeleteAsync(id);
        }
    }
}

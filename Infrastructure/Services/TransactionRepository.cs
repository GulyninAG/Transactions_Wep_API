using Microsoft.EntityFrameworkCore;
using Transactions_Web_API.Domain;
using Transactions_Web_API.Infrastructure.Interfaces;

namespace Transactions_Web_API.Infrastructure.Services
{
    public class TransactionRepository : ITransactionRepository
    {
        private readonly TransactionDBContext _context;

        public TransactionRepository(TransactionDBContext context)
        {
            _context = context;
        }

        public async Task<List<Transaction>> GetAllAsync()
        {
            return await _context.Transactions.ToListAsync();
        }

        public async Task<Transaction> GetByIdAsync(Guid id)
        {
            return await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task AddAsync(Transaction transaction)
        {
            await _context.Transactions.AddAsync(transaction);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var transaction = await _context.Transactions.FirstOrDefaultAsync(t => t.Id == id);
            if (transaction != null)
            {
                _context.Transactions.Remove(transaction);
                await _context.SaveChangesAsync();
            }
        }
    }
}

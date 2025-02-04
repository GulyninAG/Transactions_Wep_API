using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Transactions_Web_API.Domain;

namespace Transactions_Web_API.Infrastructure.Services
{
    public class TransactionDBContext : DbContext
    {
        protected readonly IConfiguration _configuration;
        public DbSet<Transaction> Transactions { get; set; }

        public TransactionDBContext(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            options.UseSqlite(_configuration.GetConnectionString("TransactionWebApiDB"));
        }
    }
}

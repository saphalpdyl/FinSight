using Fin.Core.Entities;
using Fin.Core.Repositories;
using Fin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;

namespace Fin.Infrastructure.Repositories
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly Logger _logger;
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext, Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionByAccountIdAsync(int accountId)
        {
            return await _dbContext.Transactions
                .Where(t => t.Account.Id== accountId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionsByUserIdAsync(string userId)
        {
            return await _dbContext.Transactions
                .Where(t => t.Account.User.Id == userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}

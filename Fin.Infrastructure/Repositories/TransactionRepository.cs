using Fin.Core.Entities;
using Fin.Core.Repositories;
using Fin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fin.Infrastructure.Repositories
{
    public class TransactionRepository: ITransactionRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public TransactionRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Transaction>> GetAllTransactionByAccountIdAsync(int userId)
        {
            return await _dbContext.Transactions
                .Where(t => t.Account.Id== userId)
                .OrderByDescending(t => t.CreatedAt)
                .ToListAsync();
        }
    }
}

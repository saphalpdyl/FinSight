using Fin.Core.Entities;
using Fin.Core.Repositories;
using Fin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Fin.Infrastructure.Repositories
{
    public class AccountRepository: IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public AccountRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public async Task<IEnumerable<Account>> GetAllAccountsAsync(int userId)
        {
            return await _dbContext.Accounts
                .Where(a => a.Id == userId)
                .Include(a => a.Transactions
                    .OrderByDescending(t => t.CreatedAt)
                    .Take(1) // Get the most recent transaction for each account
                )
                .ToListAsync();
        }
    }
}

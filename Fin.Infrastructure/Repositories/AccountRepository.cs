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

        public async Task<IEnumerable<Account>> GetAllAccountsAsync(string userId)
        {
            return await _dbContext.Accounts
                .Where(a => a.User.Id == userId)
                .Include(a => a.Transactions
                        .Where(t => t.Account.Id == a.Id)
                    //.OrderByDescending(t => t.CreatedAt)
                    .Take(1) // Get the most recent transaction for each account
                )
                .ToListAsync();
        }
    }
}

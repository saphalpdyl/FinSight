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
                .Select(a => new Account
                {
                    Id = a.Id,
                    Name = a.Name,
                    // Include other Account properties you need
                    User = a.User,
                    Transactions = a.Transactions
                        .OrderByDescending(t => t.CreatedAt)
                        .Take(1) // This works correctly in Select
                        .ToList()
                })
                .ToListAsync();
        }
    }
}

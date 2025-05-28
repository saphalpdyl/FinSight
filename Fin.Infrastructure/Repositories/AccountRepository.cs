using Fin.Core.Entities;
using Fin.Core.Repositories;
using Fin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Serilog.Core;

namespace Fin.Infrastructure.Repositories
{
    public class AccountRepository: IAccountRepository
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly Logger _logger;

        public AccountRepository(ApplicationDbContext dbContext, Logger logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
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

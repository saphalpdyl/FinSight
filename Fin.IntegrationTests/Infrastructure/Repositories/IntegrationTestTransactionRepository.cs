using Fin.Core.Entities;
using Fin.Infrastructure.Repositories;
using Fin.IntegrationTests.Bases;
using Xunit;

namespace Fin.IntegrationTests.Infrastructure.Repositories
{
    public sealed class IntegrationTestTransactionRepository: PostgresIntegrationBase, IAsyncLifetime
    {
        private readonly int TEST_ACCOUNT_ID = 101;
        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            var fakeUser = new FinsightUser();

            _dbContext.Users.Add(fakeUser);
            await _dbContext.SaveChangesAsync();

            var accountsData = new List<Account>
            {
                new Account
                {
                    Id = TEST_ACCOUNT_ID,
                    Name = "Main Checking",
                    User = fakeUser, // Link this account to the fakeUser
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 10, 10, 0, 0, DateTimeKind.Utc), Amount = 100m, Description = "Deposit 1", IsDebit = false },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc), Amount = -20m, Description = "Withdrawal 1", IsDebit = true },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 20, 14, 0, 0, DateTimeKind.Utc), Amount = 50m, Description = "Deposit 2 (Latest)", IsDebit = false } // Latest transaction for Account 101
                    }
                }
            };

            _dbContext.Accounts.AddRange(accountsData);
            await _dbContext.SaveChangesAsync();
        }

        [Fact]
        public async Task GetAllTransactionByAccountIdAsync_ShouldReturnTransactionsForGivenAccountId()
        {
            var transactionRepository = new TransactionRepository(_dbContext);

            var resultTransactions = await transactionRepository.GetAllTransactionByAccountIdAsync(TEST_ACCOUNT_ID);

            // Assertions
            Assert.NotNull(resultTransactions);
            resultTransactions = resultTransactions.ToList();
            Assert.Equal(3, resultTransactions.Count());
        }
    }
}

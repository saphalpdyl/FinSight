using Fin.Core.Entities;
using Fin.Infrastructure.Repositories;
using Fin.IntegrationTests.Bases;
using Xunit;

namespace Fin.IntegrationTests.Infrastructure.Repositories
{
    public sealed class IntegrationTestTransactionRepository: PostgresIntegrationBase, IAsyncLifetime
    {
        private readonly int TEST_ACCOUNT_ID = 101;
        private readonly FinsightUser _fakeUser = new FinsightUser();
        private readonly FinsightUser _anotherFakeUser = new FinsightUser();
        public async Task InitializeAsync()
        {
            await base.InitializeAsync();

            _dbContext.Users.Add(_fakeUser);
            await _dbContext.SaveChangesAsync();

            var accountsData = new List<Account>
            {
                new Account
                {
                    Id = 101,
                    Name = "Main Checking",
                    User = _fakeUser, // Link this account to the fakeUser
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 10, 10, 0, 0, DateTimeKind.Utc), Amount = 100m, Description = "Deposit 1", IsDebit = false },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc), Amount = -20m, Description = "Withdrawal 1", IsDebit = true },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 20, 14, 0, 0, DateTimeKind.Utc), Amount = 50m, Description = "Deposit 2 (Latest)", IsDebit = false } // Latest transaction for Account 101
                    }
                },
                new Account
                {
                    Id = 102,
                    Name = "Emergency Savings",
                    User = _fakeUser, // Link this account to the fakeUser
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 2, 1, 9, 0, 0, DateTimeKind.Utc), Amount = 500m, Description = "Initial Deposit", IsDebit = false },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 2, 5, 11, 0, 0, DateTimeKind.Utc), Amount = -100m, Description = "Transfer Out (Latest)", IsDebit = true } // Latest transaction for Account 102
                    }
                },
                new Account
                {
                    Id = 103,
                    Name = "Empty Account",
                    User = _fakeUser, // Link this account to the fakeUser
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>() // Account with no transactions
                },
                new Account
                {
                    Id = 104,
                    Name = "Another User's Account",
                    User = _anotherFakeUser, // This account belongs to another user
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 3, 1, 8, 0, 0, DateTimeKind.Utc), Amount = 200m, Description = "Deposit", IsDebit = false }
                    }
                },
                new Account
                {
                    Id = 105,
                    Name = "Yet Another User's Account",
                    User = _anotherFakeUser, // This account belongs to another user
                    TransactionsCachedUntilDateTime = DateTime.UtcNow,
                    Transactions = new List<Transaction>()
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

        [Fact]
        public async Task GetAllTransactionsByUserIdAsync_ShouldReturnTransactionsForGivenUserId()
        {
            var transactionRepository = new TransactionRepository(_dbContext);
            if (_fakeUser == null)
            {
                throw new InvalidOperationException("No user found in the database for testing.");
            }

            var resultTransactions = await transactionRepository.GetAllTransactionsByUserIdAsync(_fakeUser.Id);
            // Assertions
            Assert.NotNull(resultTransactions);
            resultTransactions = resultTransactions.ToList();
            Assert.Equal(5, resultTransactions.Count()); // Should return all transactions for the fake user
        }
    }
}

using Fin.Core.Entities;
using Fin.IntegrationTests.Bases;
using Fin.Infrastructure.Repositories;
using Serilog;
using Serilog.Core;
using Xunit;

namespace Fin.IntegrationTests.Infrastructure.Repositories
{
    public sealed class IntegrationTestAccountRepository: PostgresIntegrationBase
    {

        [Fact]
        public async Task GetAllAccountsAsync_ShouldReturnAccountsWithOneTransactionMax()
        {
            var fakeUser = new FinsightUser();
            var anotherFakeUser = new FinsightUser();

            //_dbContext..AddRange(fakeUser, anotherFakeUser);
            _dbContext.Users.Add(fakeUser);
            await _dbContext.SaveChangesAsync();

            var accountsData = new List<Account>
            {
                new Account
                {
                    Id = 101,
                    Name = "Main Checking",
                    User = fakeUser, // Link this account to the fakeUser
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
                    User = fakeUser, // Link this account to the fakeUser
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
                    User = fakeUser, // Link this account to the fakeUser
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>() // Account with no transactions
                },
                new Account
                {
                    Id = 104,
                    Name = "Another User's Account",
                    User = anotherFakeUser, // This account belongs to another user
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
                    User = anotherFakeUser, // This account belongs to another user
                    TransactionsCachedUntilDateTime = DateTime.UtcNow,
                    Transactions = new List<Transaction>()
                }
            };

            _dbContext.Accounts.AddRange(accountsData);
            await _dbContext.SaveChangesAsync();

            var accountRepository = new AccountRepository(_dbContext, _logger);

            var result = await accountRepository.GetAllAccountsAsync(fakeUser.Id);

            Assert.NotNull(result);
            result = result.ToList();

            Assert.Equal(3, result.Count());
            Assert.All(result, a =>
            {
                Assert.NotNull(a.Transactions);
                Assert.True(a.Transactions.Count() <= 1, "Expected at most 1 transactions per account");
            });
        }
    }
}

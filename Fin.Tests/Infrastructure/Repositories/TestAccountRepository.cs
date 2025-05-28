using Fin.Core.Entities;
using Fin.Infrastructure.Repositories;
using Fin.Tests.Helpers.Mocking;
using Xunit;

namespace Fin.Tests.Infrastructure.Repositories
{
    public sealed class TestAccountRepository
    {
        [Fact]
        public async Task GetAllAccountsAsync_ShouldReturnAccounts()
        {
            var fakeUser = new FinsightUser();
            var anotherFakeUser = new FinsightUser();
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

            foreach (var account in accountsData)
            {
                foreach (var transaction in account.Transactions)
                {
                    transaction.Account = account; // Set the back-reference
                }
            }

            var transactionsData = accountsData.SelectMany(a => a.Transactions).ToList();

            var mockDbContext = DbTestHelpers.BuildMockApplicationDbContext(accountsData, transactionsData);
            var repository = new AccountRepository(mockDbContext.Object);

            var result = await repository.GetAllAccountsAsync(fakeUser.Id);

            // Assertions
            Assert.NotNull(result);
            result = result.ToList();

            Assert.Equal(3, result.Count());
        }
    }
}

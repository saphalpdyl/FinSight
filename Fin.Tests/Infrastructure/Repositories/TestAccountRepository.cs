using Fin.Core.Entities;
using Fin.Infrastructure.Repositories;
using Fin.Tests.Helpers.Mocking;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Fin.Tests.Infrastructure.Repositories
{
    public class TestAccountRepository
    {
        [Fact]
        public async Task GetAllAccountsAsync_ShouldReturnAccountsWithOneTransactionsMax()
        {
            const int testUserId = 101;
            var accountsData = new List<Account>
            {
                new Account
                {
                    Id = 101,
                    Name = "Main Checking",
                    // UserId removed as per your Account entity definition
                    TransactionsCachedUntilDateTime = DateTime.MinValue, // Default value as per definition
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 10, 10, 0, 0, DateTimeKind.Utc), Amount = 100m, Description = "Deposit 1", IsDebit = false },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 15, 12, 0, 0, DateTimeKind.Utc), Amount = -20m, Description = "Withdrawal 1", IsDebit = true },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 1, 20, 14, 0, 0, DateTimeKind.Utc), Amount = 50m, Description = "Deposit 2 (Latest)", IsDebit = false }
                    }
                },
                new Account
                {
                    Id = 102,
                    Name = "Emergency Savings",
                    // UserId removed
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 2, 1, 9, 0, 0, DateTimeKind.Utc), Amount = 500m, Description = "Initial Deposit", IsDebit = false },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 2, 5, 11, 0, 0, DateTimeKind.Utc), Amount = -100m, Description = "Transfer Out (Latest)", IsDebit = true }
                    }
                },
                new Account
                {
                    Id = 103,
                    Name = "Empty Account",
                    // UserId removed
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>()
                },
                new Account
                {
                    Id = 104,
                    Name = "Another Generic Account", // Renamed as "Another User's Account" implies UserId
                    // UserId removed
                    TransactionsCachedUntilDateTime = DateTime.MinValue,
                    Transactions = new List<Transaction>
                    {
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 3, 1, 8, 0, 0, DateTimeKind.Utc), Amount = 75m, Description = "Their Tx 1" },
                        new Transaction { Id = Guid.NewGuid(), CreatedAt = new DateTime(2025, 3, 5, 9, 0, 0, DateTimeKind.Utc), Amount = -25m, Description = "Their Tx 2 (Latest)" }
                    }
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

            var result = await repository.GetAllAccountsAsync(testUserId);

            // Assertions
            Assert.NotNull(result);
            result = result.ToList();

            Assert.Equal(3, result.Count());

            Assert.All(result, a =>
            {
                Assert.True(a.Id == testUserId);
                Assert.NotNull(a.Transactions);
                Assert.True(a.Transactions.Count() <= 1,
                    $"Account {a.Id} has {a.Transactions.Count} transactions, expected at most 1.");
            });
        }
    }
}

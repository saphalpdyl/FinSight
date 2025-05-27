using Fin.Core.Entities;
using Fin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;
using Moq;

namespace Fin.Tests.Helpers.Mocking
{
    public class DbTestHelpers
    {
        /// <summary>
        /// Creates a mock DbSet for unit testing EF Core queries against in-memory data,
        /// correctly supporting asynchronous LINQ operations (ToListAsync, etc.).
        /// Requires the 'MockQueryable.Moq' NuGet package.
        /// </summary>
        /// <typeparam name="T">The entity type of the DbSet.</typeparam>
        /// <param name="data">The in-memory data to be used by the mock DbSet.</param>
        /// <returns>A Mock<DbSet<T>> instance configured to query the provided data.</returns>
        public static Mock<DbSet<T>> BuildMockDbSet<T>(IEnumerable<T> data) where T : class
        {
            // This is the magic line that uses MockQueryable.Moq's extension!
            return data.AsQueryable().BuildMockDbSet();
        }

        /// <summary>
        /// Creates a mock ApplicationDbContext configured with provided in-memory data for its DbSets.
        /// This helper assumes your ApplicationDbContext has DbSets for Account, Transaction, and Product.
        /// Adjust the parameters and internal setup as per your actual DbContext properties.
        /// </summary>
        /// <param name="accounts">Initial data for the Accounts DbSet.</param>
        /// <param name="transactions">Initial data for the Transactions DbSet.</param>
        /// <returns>A Mock\<ApplicationDbContext\> instance.</returns>
        public static Mock<ApplicationDbContext> BuildMockApplicationDbContext(
            IEnumerable<Account> accounts,
            IEnumerable<Transaction> transactions)
        {
            // Create mock DbSets using our BuildMockDbSet helper
            var mockAccountsDbSet = BuildMockDbSet(accounts);
            var mockTransactionsDbSet = BuildMockDbSet(transactions);

            // Mock the DbContext itself.
            // We need to pass DbContextOptions to the constructor of ApplicationDbContext,
            // even if they are just dummy options for mocking purposes.
            var mockContext = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>());

            // Set up each DbSet property on the mock context to return our mock DbSets
            mockContext.Setup(c => c.Accounts).Returns(mockAccountsDbSet.Object);
            mockContext.Setup(c => c.Transactions).Returns(mockTransactionsDbSet.Object);

            // Set up SaveChangesAsync to return a completed task (or a specific integer).
            // This is crucial if your repository calls SaveChangesAsync.
            mockContext.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()))
                       .ReturnsAsync(1); // Returns 1 indicating one entity was saved (or more)

            return mockContext;
        }
    }
}

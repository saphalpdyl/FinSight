using Fin.Core.Entities;
using Fin.Core.Services;
using Fin.Infrastructure.Repositories;

namespace Fin.Application.Services
{
    public class RemoteTransactionService(
        AccountRepository accountRepository,
        TransactionRepository transactionRepository) : IRemoteTransactionService
    {
        private readonly AccountRepository _accountRepository = accountRepository ?? throw new ArgumentNullException(nameof(accountRepository));
        private readonly TransactionRepository _transactionRepository = transactionRepository ?? throw new ArgumentNullException(nameof(transactionRepository));

        public async Task<ICollection<Transaction>> GetTransactionsByUserIdAsync(string userId,
            bool cacheTransactionsToLocalDatabase = true)

        {
            // Get transactions from remote
            // Delete previous transactions for the user
            // Cache new transaction to local database
            return await Task.FromResult(new List<Transaction>());
        }
    }
}

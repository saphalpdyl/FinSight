using Fin.Core.Entities;

namespace Fin.Core.Services
{
    /// <summary>
    /// Defines methods for managing remote banking API call and data caching operations.
    /// </summary>
    public interface IRemoteTransactionService
    {
        Task<ICollection<Transaction>> GetTransactionsByUserIdAsync(string userId, bool cacheTransactionsToLocalDatabase);
    }
}

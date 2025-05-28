using Fin.Core.Entities;

namespace Fin.Core.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactionByAccountIdAsync(int accountId);
        Task<IEnumerable<Transaction>> GetAllTransactionsByUserIdAsync(string userId);
    }
}

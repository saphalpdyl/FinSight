using Fin.Core.Entities;

namespace Fin.Core.Repositories
{
    public interface ITransactionRepository
    {
        Task<IEnumerable<Transaction>> GetAllTransactionByAccountIdAsync(int userId);
    }
}

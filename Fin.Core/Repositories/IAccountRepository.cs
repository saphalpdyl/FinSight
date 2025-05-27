using Fin.Core.Entities;

namespace Fin.Core.Repositories
{
    public interface IAccountRepository
    {
        Task<IEnumerable<Account>> GetAllAccountsAsync(int userId);
    }
}

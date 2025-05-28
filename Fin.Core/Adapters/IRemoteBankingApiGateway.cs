using Fin.Core.Entities;

namespace Fin.Core.Adapters
{
    public interface IRemoteBankingApiGateway
    {
        Task<IEnumerable<Transaction>> GetTransactionsWithUserIdFromRemoteAsync(string userId);
    }
}

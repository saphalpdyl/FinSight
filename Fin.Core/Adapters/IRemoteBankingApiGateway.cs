using Fin.Core.Entities;

namespace Fin.Core.Adapters
{
    public interface IRemoteBankingApiGateway
    {
        Task<IEnumerable<Transaction>> GetTransactionsFromAccountsWithUserIdFromRemoteAsync(string userId,
            string externalAccountId, int internalAccountId);
    }
}

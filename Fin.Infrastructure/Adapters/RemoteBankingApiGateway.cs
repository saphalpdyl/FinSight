using Fin.Core.Adapters;
using Fin.Core.Entities;
using Serilog.Core;

namespace Fin.Infrastructure.Adapters
{
    public class RemoteBankingApiGateway(HttpClient httpClient, Logger logger): IRemoteBankingApiGateway
    {
        private readonly Logger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
        public async Task<IEnumerable<Transaction>> GetTransactionsFromAccountsWithUserIdFromRemoteAsync(string userId,
            string externalAccountId)
        {
            string requestUri = $"transactions?userId={userId}";

            // TODO: In the service, verify that account with ID: externalAccountId exists for userId

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                var json = await response.Content.ReadAsStringAsync();
            }
            catch (HttpIOException ex)
            {
                _logger.Error($"[HTTP] Error occurred while fetching transactions for userId {userId}: {ex.Message}", ex);
            }
            catch (Exception ex)
            {
                _logger.Error($"Unexpected error occurred while fetching transactions for userId {userId}: {ex.Message}", ex);
            }

            throw new NotImplementedException();
        }
    }
}

using System.Text.Json;
using Fin.Core.Adapters;
using Fin.Core.Entities;
using Fin.Core.Models.Adapters;
using Serilog.Core;

namespace Fin.Infrastructure.Adapters
{
    public class RemoteBankingApiGateway(HttpClient httpClient, Logger logger): IRemoteBankingApiGateway
    {
        private readonly Logger _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
        public async Task<IEnumerable<Transaction>> GetTransactionsFromAccountsWithUserIdFromRemoteAsync(string userId,
            string externalAccountId, int internalAccountId)
        {
            var requestUri = $"api/v1/transactions/byAccount/?accountId={externalAccountId}";

            // TODO: In the service, verify that account with ID: externalAccountId exists for userId

            try
            {
                var response = await _httpClient.GetAsync(requestUri);
                var json = await response.Content.ReadAsStringAsync();

                var responseModel =
                    JsonSerializer.Deserialize<RemoteBankingApiGetResponse>(json);

                if ( responseModel == null || responseModel.Transactions == null )
                {
                    _logger.Warning($"[HTTP] No transactions found for userId {userId} and accountId {externalAccountId}");
                    throw new InvalidOperationException("No transactions found in the response.");
                }

                responseModel.Transactions.Select(t =>
                {
                    return new Transaction
                    {
                        ExternalId = t.AccountId,
                        AccountId = internalAccountId,
                        Amount = t.Amount,
                        CreatedAt = t.CreatedAt,
                        Description = t.Description,
                        IsDebit = t.IsDebit,
                    };
                });
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

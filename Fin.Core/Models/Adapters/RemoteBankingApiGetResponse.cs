using System.Text.Json.Serialization;

namespace Fin.Core.Models.Adapters
{
    public class RemoteBankingApiGetResponse
    {
        [JsonPropertyName("accountId")] public string AccountId { get; set; }
        [JsonPropertyName("transactions")] public IEnumerable<RemoteTransactionResponse> Transactions { get; set; }
    }

    public class RemoteTransactionResponse
    {
        [JsonPropertyName("id")] public string Id { get; set; }
        [JsonPropertyName("createdAt")] public DateTime CreatedAt{ get; set; }
        [JsonPropertyName("description")] public string Description { get; set; }
        [JsonPropertyName("amount")] public decimal Amount { get; set; }
        [JsonPropertyName("accountId")] public string AccountId { get; set; }
        [JsonPropertyName("isDebit")] public bool IsDebit { get; set; }
    }
}

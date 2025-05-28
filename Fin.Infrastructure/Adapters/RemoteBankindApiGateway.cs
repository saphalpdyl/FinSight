using Fin.Core.Adapters;
using Fin.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Serilog.Core;

namespace Fin.Infrastructure.Adapters
{
    public class RemoteBankindApiGateway(HttpClient _httpClient, Logger _logger): IRemoteBankingApiGateway
    {
        private readonly Logger _logger = Logger ?? throw new ArgumentNullException(nameof(Logger));
        private readonly HttpClient _httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            
        public Task<IEnumerable<Transaction>> GetTransactionsWithUserIdFromRemoteAsync(string userId)
        {
            string requestUri = $"transactions?userId={userId}";

            try
            {

            }
            catch (Exception ex)
            {

            }
        }
    }
}

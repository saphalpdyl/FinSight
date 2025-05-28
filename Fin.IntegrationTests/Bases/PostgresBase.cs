using Testcontainers.PostgreSql;
using Xunit;

namespace Fin.IntegrationTests.Bases
{
    public class PostgresBase: IAsyncLifetime
    {
        protected readonly PostgreSqlContainer _postgresContainer = new PostgreSqlBuilder()
            .WithImage("postgres:15-alpine")
            .Build();

        public Task InitializeAsync()
        {
            return _postgresContainer.StartAsync();
        }

        public Task DisposeAsync()
        {
            return _postgresContainer.DisposeAsync().AsTask();
        }
    }
}

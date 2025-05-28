using Fin.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Testcontainers.PostgreSql;
using Xunit;

namespace Fin.IntegrationTests.Bases
{
    public class PostgresIntegrationBase: IAsyncLifetime
    {
        protected PostgreSqlContainer _postgresContainer;

        protected ApplicationDbContext _dbContext;

        public PostgresIntegrationBase()
        {
        }

        public async Task InitializeAsync()
        {
            _postgresContainer = new PostgreSqlBuilder()
                .WithImage("postgres:15-alpine")
                .Build();

            await _postgresContainer.StartAsync();

            var connectionString = _postgresContainer.GetConnectionString();
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseNpgsql(connectionString) // Use UseNpgsql for PostgreSQL
                .Options;

            _dbContext = new ApplicationDbContext(options);

            await _dbContext.Database.MigrateAsync();
        }

        public async Task DisposeAsync()
        {
            await _postgresContainer.StopAsync();
            await _postgresContainer.DisposeAsync();

            await _dbContext.DisposeAsync();
        }
    }
}

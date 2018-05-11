using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Data;

namespace CodefictionApi.IntegrationTests
{
    public class InMemoryDatabaseProvider : IDatabaseProvider
    {
        private readonly Database _database;

        public InMemoryDatabaseProvider(Database database)
        {
            _database = database;
        }

        public async Task<Database> GetDatabase()
        {
            return await Task.FromResult<Database>(_database);
        }
    }
}

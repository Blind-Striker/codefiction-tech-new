using System;
using System.IO;
using System.Threading.Tasks;
using CodefictionApi.Core.Contracts;
using CodefictionApi.Core.Data;
using Microsoft.Extensions.Caching.Memory;
using Newtonsoft.Json;

namespace CodefictionApi.Core.Repositories
{
    public class DatabaseProvider : IDatabaseProvider
    {
        private const string DbCacheKey = "database";

        private readonly IMemoryCache _memoryCache;
        private readonly string _dbSource;

        public DatabaseProvider(IMemoryCache memoryCache, string dbSource)
        {
            _memoryCache = memoryCache;
            _dbSource = dbSource;
        }

        public async Task<Database> GetDatabase()
        {
            if (_memoryCache.TryGetValue(DbCacheKey, out Database database))
            {
                return database;
            }

            using (StreamReader reader = File.OpenText(_dbSource))
            {
                string databaseStr = await reader.ReadToEndAsync();
                database = JsonConvert.DeserializeObject<Database>(databaseStr);
            }
           
            MemoryCacheEntryOptions cacheEntryOptions = new MemoryCacheEntryOptions().SetAbsoluteExpiration(TimeSpan.FromHours(1));
            _memoryCache.Set(DbCacheKey, database, cacheEntryOptions);

            return database;
        }
    }
}

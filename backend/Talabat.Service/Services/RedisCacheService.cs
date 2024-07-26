using StackExchange.Redis;
using System.Threading.Tasks;

namespace Connectify.Core.Services
{
    public class RedisCacheService
    {
        private readonly IConnectionMultiplexer _redis;
        private readonly IDatabase _database;

        public RedisCacheService(IConnectionMultiplexer redis)
        {
            _redis = redis;
            _database = _redis.GetDatabase();
        }

		public async Task SetStringAsync(string key, string value)
		{
			await _database.StringSetAsync(key, value);
		}
        public async Task UpdateStringAsync(string key, string value)
        {
            await _database.StringAppendAsync(key, value);
        }
        public async Task<string> GetStringAsync(string key)
		{
			return await _database.StringGetAsync(key);
		}
        public async Task<bool> KeyExistsAsync(string key)
        {
            return await _database.KeyExistsAsync(key);
        }

        public async Task RemoveAsync(string key)
        {
            await _database.KeyDeleteAsync(key);
        }
	}
}

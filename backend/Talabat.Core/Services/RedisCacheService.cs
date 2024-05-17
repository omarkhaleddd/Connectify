using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
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

		public async Task<string> GetStringAsync(string key)
		{
			return await _database.StringGetAsync(key);
		}
	}
}

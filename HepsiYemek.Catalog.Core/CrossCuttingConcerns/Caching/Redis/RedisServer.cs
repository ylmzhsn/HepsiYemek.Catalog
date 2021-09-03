namespace HepsiYemek.Catalog.Core.CrossCuttingConcerns.Caching.Redis
{
    using Microsoft.Extensions.Configuration;
    using StackExchange.Redis;

    /// <summary>
    /// Redis server initialize
    /// </summary>
    public class RedisServer
    {
        private ConnectionMultiplexer _connectionMultiplexer;
        private IDatabase _database;
        private string configurationString;
        private int _currentDatabaseId = 0;

        public RedisServer(IConfiguration configuration)
        {
            CreateRedisConfigurationString(configuration);

            _connectionMultiplexer = ConnectionMultiplexer.Connect(configurationString);
            _database = _connectionMultiplexer.GetDatabase(_currentDatabaseId);
        }

        public IDatabase Database => _database;

        public void FlushDatabase()
        {
            _connectionMultiplexer.GetServer(configurationString).FlushDatabase(_currentDatabaseId);
        }

        private void CreateRedisConfigurationString(IConfiguration configuration)
        {
            var host = configuration.GetSection("Redis:Hosts:0:Host").Value;

            var port = configuration.GetSection("Redis:Hosts:0:Port").Value;

            configurationString = $"{host}:{port}";
        }
    }
}

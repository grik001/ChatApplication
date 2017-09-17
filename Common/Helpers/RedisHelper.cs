using Common.Helpers.IHelpers;
using Newtonsoft.Json;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class RedisHelper
    {
        IApplicationConfig _applicationConfig = null;

        public RedisHelper(IApplicationConfig applicationConfig)
        {
            this._applicationConfig = applicationConfig;
        }

        public IDatabase GetConnection()
        {
            var serverName = _applicationConfig.RedisServerName;
            var connection = ConnectionMultiplexer.Connect($"{serverName},allowAdmin=true");

            var server = connection.GetServer(serverName);
            var database = connection.GetDatabase();

            return database;
        }

        public T GetValue<T>(string key, string value)
        {
            var database = GetConnection();
            var data = database.StringGet(key);
            var result = JsonConvert.DeserializeObject<T>(data);
            return result;
        }

        public bool SetValue<T>(string key, T value)
        {
            var database = GetConnection();
            var result = database.StringSet(key, JsonConvert.SerializeObject(value));
            return result;
        }
    }
}

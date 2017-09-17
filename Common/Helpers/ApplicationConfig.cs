using Common.Helpers.IHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class ApplicationConfig : IApplicationConfig
    {
        public string QueueName { get => "task_queue3"; }

        public string RabbitConnection { get => "localhost"; }

        public string RedisServerName  { get => "192.168.99.100:32770"; }
}
}

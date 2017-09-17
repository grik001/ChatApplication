using Common;
using Common.Helpers;
using Common.Helpers.IHelpers;
using Common.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebApplication.Service
{
    class Program
    {
        static IApplicationConfig _applicationConfig = null;
        static IMessageQueueHelper _messageQueueHelper = null;

        static void Main(string[] args)
        {
            _applicationConfig = new ApplicationConfig();
            _messageQueueHelper = new RabbitMQHelper();

            Task.Run(() => _messageQueueHelper.ReadMessages<QueueMetaData>(_applicationConfig, ProcessQueue));

            Console.ReadLine();
        }

        public static void ProcessQueue(QueueMetaData queueMetaData)
        {
            Console.WriteLine(queueMetaData.ChatEntryTime);
        }

    }
}

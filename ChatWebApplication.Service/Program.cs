using Autofac;
using ChatWebApplication.Helpers.Service;
using ChatWebApplication.Service.Helpers;
using Common;
using Common.Data;
using Common.Data.IData;
using Common.Helpers;
using Common.Helpers.IHelpers;
using Common.Models;
using Microsoft.Owin.Hosting;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatWebApplication.Service
{
    class Program
    {
        static IApplicationConfig _applicationConfig = null;
        static IMessageQueueHelper _messageQueueHelper = null;
        static ICacheHelper _cacheHelper = null;
        static IQueueDataModel _queueDataModel = null;
        static IAgentDataModel _agentDataModel = null;

        static void Main(string[] args)
        {
            #region DI Container Builder
            var builder = new ContainerBuilder();

            builder.RegisterType<RedisHelper>().As<ICacheHelper>();
            builder.RegisterType<ApplicationConfig>().As<IApplicationConfig>();
            builder.RegisterType<RabbitMQHelper>().As<IMessageQueueHelper>();
            builder.RegisterType<QueueDataModel>().As<IQueueDataModel>();
            builder.RegisterType<AgentDataModel>().As<IAgentDataModel>();

            var container = builder.Build();
            #endregion

            using (var scope = container.BeginLifetimeScope())
            {
                #region DI Resolver
                _cacheHelper = scope.Resolve<ICacheHelper>();
                _messageQueueHelper = scope.Resolve<IMessageQueueHelper>();
                _applicationConfig = scope.Resolve<IApplicationConfig>();
                _queueDataModel = scope.Resolve<IQueueDataModel>();
                _agentDataModel = scope.Resolve<IAgentDataModel>();

                var chatMonitorHelper = new ChatMonitorHelper(_queueDataModel, _agentDataModel);
                var webserverHelper = new WebServerHelper();
                #endregion

                #region Processors
                //Start Message Queue Listener
                Task.Run(() => _messageQueueHelper.ReadMessages<QueueMetaData>(_applicationConfig, chatMonitorHelper.ProcessQueue));
                //Start Message Queue Processor
                Task.Run(() => chatMonitorHelper.MonitorQueue());
                //Start WebSocket Server
                Task.Run(() => webserverHelper.StartWebServer());
                #endregion
            }

            Console.ReadLine();
        }


    }
}

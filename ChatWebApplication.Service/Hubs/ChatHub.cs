using Common.Data;
using Common.Data.IData;
using Common.Helpers;
using Common.Helpers.IHelpers;
using Common.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebApplication.Service.Hubs
{
    public class ChatHub : Hub
    {
        static IApplicationConfig _applicationConfig = null;
        static IMessageQueueHelper _messageQueueHelper = null;
        static ICacheHelper _cacheHelper = null;
        static IQueueDataModel _queueDataModel = null;
        static IAgentDataModel _agentDataModel = null;

        public ChatHub()
        {
            _applicationConfig = new ApplicationConfig();
            _cacheHelper = new RedisHelper(_applicationConfig);
            _queueDataModel = new QueueDataModel(_cacheHelper);
            _agentDataModel = new AgentDataModel(_cacheHelper);
        }

        public void SendMessageToClient(string targetClient, string name, string message)
        {
            var agent = _agentDataModel.Get(new Guid(Context.ConnectionId));

            if (agent != null)
            {
                var isClientAssignedToAgent = agent.ActiveChats.Contains(new Guid(targetClient));

                if (isClientAssignedToAgent)
                {
                    Clients.Client(targetClient).addNewMessageToPage(name, message);
                    Clients.Client(Context.ConnectionId).addNewMessageToPage(name, message);
                }
            }
        }

        public void SendMessageToSupport(string name, string message)
        {
            var chatQueue = _queueDataModel.Get(new Guid(Context.ConnectionId));

            if (chatQueue.CurrentAgent.HasValue)
            {
                Clients.Client(chatQueue.CurrentAgent.ToString()).addNewMessageToPage(name, message);
                Clients.Client(Context.ConnectionId).addNewMessageToPage(name, message);
            }
            else
            {
                Clients.Client(Context.ConnectionId).addNewMessageToPage("System", "Please wait no Agents available");
            }
        }

        public void RegisterAgent(string name)
        {
            Agent agent = new Agent();
            agent.ID = new Guid(Context.ConnectionId);
            agent.Name = name;
            agent.Surname = "";
            agent.ServiceCount = 0;

            _agentDataModel.Insert(agent);
        }

        public override Task OnConnected()
        {
            Console.WriteLine("Hub OnConnected {0}\n", Context.ConnectionId);
            return (base.OnConnected());
        }

        public override Task OnDisconnected(bool val)
        {
            Console.WriteLine("Hub OnDisconnected {0}\n", Context.ConnectionId);
            return (base.OnDisconnected(val));
        }

        public override Task OnReconnected()
        {
            Console.WriteLine("Hub OnReconnected {0}\n", Context.ConnectionId);
            return (base.OnDisconnected(false));
        }
    }
}

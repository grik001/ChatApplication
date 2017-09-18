using ChatWebApplication.Service.Helpers;
using ChatWebApplication.Service.Hubs.IHub;
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
    public class ChatHub : Hub, IChatHub
    {
        IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();

        IQueueDataModel _queueDataModel = null;
        IAgentDataModel _agentDataModel = null;
        ICacheHelper _cacheHelper = null;
        IApplicationConfig _applicationConfig = null;

        public ChatHub()
        {
            //To Fix - DI needs to be done on startup or per request if possible
            _applicationConfig = new ApplicationConfig();
            _cacheHelper = new RedisHelper(_applicationConfig);
            _queueDataModel = new QueueDataModel(_cacheHelper);
            _agentDataModel = new AgentDataModel(_cacheHelper);
        }

        public void SendAll(string message)
        {
            hubContext.Clients.All.addNewMessageToPage("System", message);
        }

        public void SendMessageToClient(string targetClient, string name, string message)
        {
            var agent = _agentDataModel.Get(new Guid(Context.ConnectionId));

            if (agent != null)
            {
                var isClientAssignedToAgent = agent.ActiveChats.Contains(new Guid(targetClient));

                if (isClientAssignedToAgent)
                {
                    hubContext.Clients.Client(targetClient).addNewMessageToPage(name, message);
                    hubContext.Clients.Client(Context.ConnectionId).addNewMessageToPage(name, message, targetClient);
                }
            }
        }

        public void SendMessageToSupport(string name, string message)
        {
            var chatQueue = _queueDataModel.Get(new Guid(Context.ConnectionId));

            if (chatQueue.CurrentAgent.HasValue)
            {
                hubContext.Clients.Client(chatQueue.CurrentAgent.ToString()).addNewMessageToPage(name, message, Context.ConnectionId);
                hubContext.Clients.Client(Context.ConnectionId).addNewMessageToPage(name, message);
            }
            else
            {
                hubContext.Clients.Client(Context.ConnectionId).addNewMessageToPage("System", "Please wait no Agents available");
            }
        }

        public void StartAgent(string name)
        {
            AgentHelper agentHelper = new AgentHelper(_agentDataModel);
            agentHelper.RegisterAgent(name, new Guid(Context.ConnectionId));
        }

        public void StartChat(string customerID, string adminID)
        {
            hubContext.Clients.Client(adminID).startChat(customerID);

            var agent = _agentDataModel.Get(new Guid(adminID));

            hubContext.Clients.Client(adminID).addNewMessageToPage("Server", "ChatStarted with client" , customerID);
            hubContext.Clients.Client(customerID).addNewMessageToPage("Server", $"ChatStarted with {agent.Username}");
        }

        public void PauseAgent()
        {
            AgentHelper agentHelper = new AgentHelper(_agentDataModel);
            agentHelper.PauseAgent(new Guid(Context.ConnectionId));
        }

        #region Overrides
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
        #endregion
    }
}

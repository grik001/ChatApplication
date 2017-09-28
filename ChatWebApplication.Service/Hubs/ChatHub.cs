using ChatWebApplication.Service.Helpers;
using ChatWebApplication.Service.Hubs.IHub;
using Common;
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

        public ChatHub(IApplicationConfig applicationConfig, ICacheHelper cacheHelper, IQueueDataModel queueDataModel,
            IAgentDataModel agentDataModel)
        {
            _applicationConfig = applicationConfig;
            _cacheHelper = cacheHelper;
            _queueDataModel = queueDataModel;
            _agentDataModel = agentDataModel;
        }

        public void SendAll(string message)
        {
            hubContext.Clients.All.addNewMessageToPage("System", message);
        }

        public void SendMessageToClient(string targetClient, string name, string message)
        {
            var cookies = this.Context.RequestCookies;
            var adminID = this.Context.RequestCookies["AdminID"].Value;

            var agent = _agentDataModel.Get(new Guid(adminID));

            if (agent != null)
            {
                var isClientAssignedToAgent = agent.ActiveChats.Contains(new Guid(targetClient));

                if (isClientAssignedToAgent)
                {
                    IncreaseChatExpiry(targetClient);
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
                var agent = _agentDataModel.Get(chatQueue.CurrentAgent.Value);

                IncreaseChatExpiry(Context.ConnectionId);
                hubContext.Clients.Client(agent.SocketID.ToString()).addNewMessageToPage(name, message, Context.ConnectionId);
                hubContext.Clients.Client(Context.ConnectionId).addNewMessageToPage(name, message);
            }
            else
            {
                hubContext.Clients.Client(Context.ConnectionId).addNewMessageToPage("System", "Please wait no Agents available");
            }
        }

        private void IncreaseChatExpiry(string chatID)
        {
            var chatQueue = _queueDataModel.Get(new Guid(chatID));
            chatQueue.ExpiryTime = chatQueue.ExpiryTime.AddMinutes(Constants.DefaultChatExpiry);
            _queueDataModel.Update(chatQueue);
        }

        public void StartChat(string customerID, string adminID)
        {
            var agent = _agentDataModel.Get(new Guid(adminID));
            var chat = _queueDataModel.Get(new Guid(customerID));



            hubContext.Clients.Client(agent.SocketID.ToString()).startChat(customerID, chat.Username);

            hubContext.Clients.Client(agent.SocketID.ToString()).addNewMessageToPage("Server", "ChatStarted with client" , customerID);
            hubContext.Clients.Client(customerID).addNewMessageToPage("Server", $"ChatStarted with {agent.Username}");
        }

        public void NotifyAgentDisconnect(string customerID)
        {
            hubContext.Clients.Client(customerID).addNewMessageToPage("System", "Agent disconnected. Please wait to be reconnected");
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

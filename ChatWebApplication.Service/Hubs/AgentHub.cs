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
    public class AgentHub : Hub, IAgentHub
    {
        IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<AgentHub>();

        IAgentDataModel _agentDataModel = null;
        IQueueDataModel _queueDataModel = null;
        IChatHub _chatHub = null;

        public AgentHub(IAgentDataModel agentDataModel, IQueueDataModel queueDataModel, IChatHub chathub)
        {
            _agentDataModel = agentDataModel;
            _queueDataModel = queueDataModel;
            _chatHub = chathub;
        }

        public void StartAgent(string name, bool isRefresh = false)
        {
            var cookies = this.Context.RequestCookies;
            var adminID = this.Context.RequestCookies["AdminID"].Value;

            AgentHelper agentHelper = new AgentHelper(_agentDataModel, _queueDataModel, _chatHub);
            var agent = agentHelper.RegisterAgent(name, new Guid(Context.ConnectionId), new Guid(adminID));

            if(agent != null && isRefresh)
            {
                foreach (var activeChat in agent.ActiveChats)
                {
                    _chatHub.StartChat(activeChat.ToString(), agent.ID.ToString());
                }
            }
        }

        public void PauseAgent()
        {
            AgentHelper agentHelper = new AgentHelper(_agentDataModel, _queueDataModel, _chatHub);
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
            AgentHelper agentHelper = new AgentHelper(_agentDataModel, _queueDataModel, _chatHub);
            agentHelper.DisableAgent(new Guid(Context.ConnectionId));

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

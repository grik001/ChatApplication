using ChatWebApplication.Service.Hubs.IHub;
using Common.Data.IData;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebApplication.Service.Helpers
{
    public class AgentHelper
    {
        IAgentDataModel _agentDataModel = null;
        IQueueDataModel _queueDataModel = null;
        IChatHub _chatHub = null;

        public AgentHelper(IAgentDataModel agentDataModel, IQueueDataModel queueDataModel, IChatHub chatHub)
        {
            this._agentDataModel = agentDataModel;
            this._queueDataModel = queueDataModel;
            this._chatHub = chatHub;
        }

        public Agent RegisterAgent(string name, Guid socketID, Guid id)
        {
            var agent = _agentDataModel.Get(id);

            if (agent == null)
            {
                agent = new Agent();
                agent.ID = new Guid(id.ToString());
                agent.SocketID = socketID;
                agent.Username = name;
                agent.ServiceCount = 0;

                _agentDataModel.Insert(agent);
            }
            else
            {
                agent.IsPaused = false;
                agent.SocketID = socketID;
                _agentDataModel.Update(agent);
            }

            return agent;
        }

        public void DisableAgent(Guid id)
        {
            var agent = _agentDataModel.Get(id);

            foreach (var chatQueueID in agent.ActiveChats)
            {
                var chat = _queueDataModel.Get(chatQueueID);

                if (chat != null)
                {
                    chat.CurrentAgent = null;
                    _chatHub.NotifyAgentDisconnect(chat.ClientID.ToString());
                }
            }
        }
        
        public void PauseAgent(Guid id)
        {
            var agent = _agentDataModel.Get(id);

            if (agent != null)
            {
                agent.IsPaused = true;
                _agentDataModel.Update(agent);
            }
        }
    }
}

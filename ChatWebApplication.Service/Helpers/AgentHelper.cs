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

        public AgentHelper(IAgentDataModel agentDataModel)
        {
            this._agentDataModel = agentDataModel;
        }

        public void RegisterAgent(string name, Guid id)
        {
            var agent = _agentDataModel.Get(id);

            if (agent == null)
            {
                agent = new Agent();
                agent.ID = new Guid(id.ToString());
                agent.Username = name;
                agent.ServiceCount = 0;

                _agentDataModel.Insert(agent);
            }
            else
            {
                agent.IsPaused = false;
                _agentDataModel.Update(agent);
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

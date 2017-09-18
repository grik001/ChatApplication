using Common.Data.IData;
using Common.Helpers;
using Common.Helpers.IHelpers;
using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data
{
    public class AgentDataModel : IAgentDataModel
    {
        ICacheHelper _cacheHelper = null;

        public AgentDataModel(ICacheHelper cacheHelper)
        {
            this._cacheHelper = cacheHelper;
        }

        public Agent Get(Guid id)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid,Agent>>(Constants.CacheKeys.AgentList.ToString());

            Agent agent = null;
            list.TryGetValue(id, out agent);

            return agent;
        }

        public Dictionary<Guid, Agent> Get()
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, Agent>>(Constants.CacheKeys.AgentList.ToString());
            return list;
        }

        public bool HasKey(Guid id)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, Agent>>(Constants.CacheKeys.AgentList.ToString());

            Agent agent = null;
            return list.ContainsKey(id);
        }

        public bool Insert(Agent agent)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, Agent>>(Constants.CacheKeys.AgentList.ToString());

            if (list == null)
                list = new Dictionary<Guid, Agent>();

            if (list.ContainsKey(agent.ID))
                return false;

            list.Add(agent.ID, agent);

            _cacheHelper.SetValue(Constants.CacheKeys.AgentList.ToString(), list);
            return true;
        }

        public bool Update(Agent agent)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, Agent>>(Constants.CacheKeys.AgentList.ToString());

            if (!list.ContainsKey(agent.ID))
                return false;

            list.Remove(agent.ID);
            list.Add(agent.ID, agent);

            _cacheHelper.SetValue(Constants.CacheKeys.AgentList.ToString(), list);
            return true;
        }

        public bool Delete(Guid id)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, Agent>>(Constants.CacheKeys.AgentList.ToString());

            if (!list.ContainsKey(id))
                return false;

            list.Remove(id);

            _cacheHelper.SetValue(Constants.CacheKeys.AgentList.ToString(), list);
            return true;
        }
    }
}

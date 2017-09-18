using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.IData
{
    public interface IAgentDataModel
    {
        Agent Get(Guid id);

        bool HasKey(Guid id);

        bool Insert(Agent queue);

        bool Update(Agent queue);

        bool Delete(Guid id);

        Dictionary<Guid, Agent> Get();
    }
}

using Common.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Data.IData
{
    public interface IQueueDataModel
    {
        Dictionary<Guid, QueueMetaData> Get();

        QueueMetaData Get(Guid id);

        bool HasKey(Guid id);

        bool Insert(QueueMetaData queue);

        bool Update(QueueMetaData queue);

        bool Delete(Guid id);
    }
}

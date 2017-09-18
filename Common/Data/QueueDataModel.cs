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
    public class QueueDataModel : IQueueDataModel
    {
        ICacheHelper _cacheHelper = null;

        public QueueDataModel(ICacheHelper cacheHelper)
        {
            this._cacheHelper = cacheHelper;
        }

        public QueueMetaData Get(Guid id)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid,QueueMetaData>>(Constants.CacheKeys.QueueMetaDataList.ToString());

            QueueMetaData queue = null;
            list.TryGetValue(id, out queue);

            return queue;
        }

        public bool HasKey(Guid id)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, QueueMetaData>>(Constants.CacheKeys.QueueMetaDataList.ToString());

            if (list == null)
                return false;

            return list.ContainsKey(id);
        }

        public bool Insert(QueueMetaData queue)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, QueueMetaData>>(Constants.CacheKeys.QueueMetaDataList.ToString());

            if (list == null)
                list = new Dictionary<Guid, QueueMetaData>();

            if (list.ContainsKey(queue.ClientID))
                return false;

            list.Add(queue.ClientID, queue);

            _cacheHelper.SetValue(Constants.CacheKeys.QueueMetaDataList.ToString(), list);
            return true;
        }

        public bool Update(QueueMetaData queue)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, QueueMetaData>>(Constants.CacheKeys.QueueMetaDataList.ToString());

            if (!list.ContainsKey(queue.ClientID))
                return false;

            list.Remove(queue.ClientID);
            list.Add(queue.ClientID, queue);

            _cacheHelper.SetValue(Constants.CacheKeys.QueueMetaDataList.ToString(), list);
            return true;
        }

        public bool Delete(Guid id)
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, QueueMetaData>>(Constants.CacheKeys.QueueMetaDataList.ToString());

            if (!list.ContainsKey(id))
                return false;

            list.Remove(id);

            _cacheHelper.SetValue(Constants.CacheKeys.QueueMetaDataList.ToString(), list);
            return true;
        }

        public Dictionary<Guid, QueueMetaData> Get()
        {
            var list = _cacheHelper.GetValue<Dictionary<Guid, QueueMetaData>>(Constants.CacheKeys.QueueMetaDataList.ToString());
            return list;
        }
    }
}

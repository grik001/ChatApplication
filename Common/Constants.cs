using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class Constants
    {
        public static double DefaultChatExpiry = 5;

        public enum MessageFunctionType
        {
            Start,
            Stop,
            Update,
            Waiting
        }

        public enum CacheKeys
        {
            QueueMetaDataList,
            AgentList
        }

        public enum QueueKey
        {
            ChatQueue,
            AgentQueue
        }
    }
}

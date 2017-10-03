using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers.IHelpers
{
    public interface IMessageQueueHelper
    {
        void PushMessage<T>(IApplicationConfig config, T message);
        Task ReadMessages<T>(IApplicationConfig config, Action<T> actionOnReceive);
    }
}

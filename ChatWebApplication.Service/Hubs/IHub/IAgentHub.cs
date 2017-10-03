using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebApplication.Service.Hubs.IHub
{
    public interface IAgentHub
    {
        Task OnConnected();
        Task OnDisconnected(bool val);
        Task OnReconnected();
    }
}

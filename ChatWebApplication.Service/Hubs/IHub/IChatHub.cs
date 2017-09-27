using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebApplication.Service.Hubs.IHub
{
    public interface IChatHub
    {
        void SendAll(string message);
        void SendMessageToClient(string targetClient, string name, string message);
        void SendMessageToSupport(string name, string message);
        void StartChat(string customerID, string adminID);
        void NotifyAgentDisconnect(string customerID);
        Task OnConnected();
        Task OnDisconnected(bool val);
        Task OnReconnected();

    }
}

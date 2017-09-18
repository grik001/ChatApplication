using ChatWebApplication.Service.Hubs;
using Common;
using Common.Data.IData;
using Common.Helpers.IHelpers;
using Common.Models;
using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebApplication.Helpers.Service
{
    public class ChatMonitorHelper
    {
        IQueueDataModel _queueDataModel = null;
        IAgentDataModel _agentDataModel = null;

        public ChatMonitorHelper(IQueueDataModel queueDataModel, IAgentDataModel agentDataModel)
        {
            _queueDataModel = queueDataModel;
            _agentDataModel = agentDataModel;
        }

        public async void MonitorQueue()
        {
            do
            {
                var chatQueues = _queueDataModel.Get();

                if (chatQueues != null && chatQueues.Any())
                {
                    var inactiveChats = chatQueues.Values.Where(x => x.CurrentAgent == null);

                    if (inactiveChats.Any())
                    {
                        var agents = _agentDataModel.Get();

                        if (agents != null && agents.Any())
                        {
                            var unservicedChats = inactiveChats.OrderBy(x => x.ChatEntryTime);

                            foreach (var unservicedChat in unservicedChats)
                            {
                                var availableAgents = agents.Values.Where(x => x.ActiveChats.Count() < x.MaxChats);
                                if (availableAgents.Any())
                                {
                                    var leastActiveAgent = availableAgents.OrderBy(x => x.ServiceCount).FirstOrDefault();
                                    unservicedChat.CurrentAgent = leastActiveAgent.ID;
                                    unservicedChat.ExpiryTime = DateTime.UtcNow.AddMinutes(Constants.DefaultChatExpiry);

                                    leastActiveAgent.ServiceCount += 1;
                                    leastActiveAgent.ActiveChats.Add(unservicedChat.ClientID);

                                    _agentDataModel.Update(leastActiveAgent);
                                    _queueDataModel.Update(unservicedChat);
                                }
                            }
                        }
                    }
                }

                await Task.Delay(5000);
            } while (true);
        }

        public void ProcessQueue(QueueMetaData queueMetaData)
        {
            Console.WriteLine(queueMetaData.ChatEntryTime);

            if (queueMetaData.Function == Constants.MessageFunctionType.Start)
            {
                var exists = _queueDataModel.HasKey(queueMetaData.ClientID);

                if (!exists)
                {
                    queueMetaData.ChatEntryTime = DateTime.UtcNow;
                    queueMetaData.ExpiryTime = DateTime.UtcNow.AddMinutes(Constants.DefaultChatExpiry);
                    queueMetaData.CurrentAgent = null;
                    queueMetaData.Function = Constants.MessageFunctionType.Waiting;

                    _queueDataModel.Insert(queueMetaData);
                }
            }

            if (queueMetaData.Function == Constants.MessageFunctionType.Stop)
            {
                IHubContext hubContext = GlobalHost.ConnectionManager.GetHubContext<ChatHub>();
                
                new ChatHub().SendAll("WOW");
                _queueDataModel.Delete(queueMetaData.ClientID);
            }

            if (queueMetaData.Function == Constants.MessageFunctionType.Update)
            {

            }
        }
    }
}

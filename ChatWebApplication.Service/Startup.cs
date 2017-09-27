using ChatWebApplication.Service.Hubs;
using Common.Data;
using Common.Helpers;
using Microsoft.AspNet.SignalR;
using Microsoft.Owin.Cors;
using Owin;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebApplication.Service
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var _applicationConfig = new ApplicationConfig();
            var _cacheHelper = new RedisHelper(_applicationConfig);
            var _queueDataModel = new QueueDataModel(_cacheHelper);
            var _agentDataModel = new AgentDataModel(_cacheHelper);
            var chatHub = new ChatHub(_applicationConfig, _cacheHelper, _queueDataModel, _agentDataModel);

            GlobalHost.DependencyResolver.Register(
                typeof(ChatHub),() => chatHub);


            GlobalHost.DependencyResolver.Register(
                typeof(AgentHub), () => new AgentHub(_agentDataModel, _queueDataModel, chatHub));

            // Branch the pipeline here for requests that start with "/signalr"
            app.Map("/signalr", map =>
            {
                map.UseCors(CorsOptions.AllowAll);

                var hubConfiguration = new HubConfiguration
                {
                    // You can enable JSONP by uncommenting line below.
                    // JSONP requests are insecure but some older browsers (and some
                    // versions of IE) require JSONP to work cross domain
                    // EnableJSONP = true
                };

                hubConfiguration.EnableDetailedErrors = true;
                map.RunSignalR(hubConfiguration);
            });
        }
    }
}

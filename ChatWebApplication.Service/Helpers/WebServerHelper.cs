using Common.Helpers.IHelpers;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChatWebApplication.Service.Helpers
{
    public class WebServerHelper
    {
        IApplicationConfig _applicationConfig = null;

        public WebServerHelper(IApplicationConfig applicationConfig)
        {
            this._applicationConfig = applicationConfig;
        }


        public void StartWebServer()
        {
            string url = _applicationConfig.WebServerUrl;

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}

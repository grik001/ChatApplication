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
        public void StartWebServer()
        {
            string url = "http://localhost:8090";

            using (WebApp.Start(url))
            {
                Console.WriteLine("Server running on {0}", url);
                Console.ReadLine();
            }
        }
    }
}

using Autofac;
using Autofac.Integration.WebApi;
using Common;
using Common.Helpers;
using Common.Helpers.IHelpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using System.Web.Http;
using System.Web.Routing;

namespace ChatWebApplication.API
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        public static IContainer Container;

        protected void Application_Start()
        {
            SetupDependancyInjection();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
        }

        public void SetupDependancyInjection()
        {
            var config = GlobalConfiguration.Configuration;
            var builder = new ContainerBuilder();

            builder.RegisterApiControllers(Assembly.GetExecutingAssembly());
            builder.RegisterType<RabbitMQHelper>().As<IMessageQueueHelper>().SingleInstance();
            builder.RegisterType<ApplicationConfig>().As<IApplicationConfig>().SingleInstance();

            Container = builder.Build();
            config.DependencyResolver = new AutofacWebApiDependencyResolver(Container);
        }
    }
}

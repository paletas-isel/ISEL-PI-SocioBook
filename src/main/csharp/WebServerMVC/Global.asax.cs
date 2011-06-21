using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using SuperSocket.SocketBase;
using SuperSocket.SocketBase.Config;
using SuperSocket.SocketEngine;
using SuperWebSocket;

namespace WebServerMVC
{
    // Note: For instructions on enabling IIS6 or IIS7 classic mode, 
    // visit http://go.microsoft.com/?LinkId=9394801

    public class MvcApplication : System.Web.HttpApplication
    {
        public static void RegisterGlobalFilters(GlobalFilterCollection filters)
        {
            filters.Add(new HandleErrorAttribute());
        }

        public static void RegisterRoutes(RouteCollection routes)
        {
            routes.IgnoreRoute("{resource}.axd/{*pathInfo}");

            routes.MapRoute(
                "Default", // Route name
                "{controller}/{action}/{id}", // URL with parameters
                new { controller = "Home", action = "Index", id = UrlParameter.Optional } // Parameter defaults
            );

        }

        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();

            RegisterGlobalFilters(GlobalFilters.Filters);
            RegisterRoutes(RouteTable.Routes);
        }

        void StartSuperWebSocketByProgramming()
        {
            var socketServer = new WebSocketServer();
            socketServer.Setup(new RootConfig(),
                new ServerConfig
                {
                    Ip = "Any",
                    Port = 4500,
                    Mode = SocketMode.Async
                }, SocketServerFactory.Instance);

            //socketServer.CommandHandler += new CommandHandler<WebSocketSession, WebSocketCommandInfo>(socketServer_CommandHandler);
            //socketServer.NewSessionConnected += new SessionEventHandler<WebSocketSession>(socketServer_NewSessionConnected);
            //socketServer.SessionClosed += new SessionClosedEventHandler<WebSocketSession>(socketServer_SessionClosed);

            Application["WebSocketPort"] = socketServer.Config.Port;

            socketServer.Start();
        }
    }
}
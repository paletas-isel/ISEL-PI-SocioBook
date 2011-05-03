using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServer.Handlers
{
    /// <summary>
    /// Summary description for MainHandler
    /// </summary>
    public class MainHandler : IHttpHandler
    {

        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";
            context.Response.Write("Hello World");
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }
}
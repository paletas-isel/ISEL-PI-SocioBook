using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServer.Handlers.Shares
{
    /// <summary>
    /// Summary description for RemoveShare
    /// </summary>
    public class RemoveShare : IHttpHandler
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
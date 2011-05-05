using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebServer.Handlers.Shares
{
    /// <summary>
    /// Summary description for GetShare
    /// </summary>
    public class GetShare : IHttpHandler
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
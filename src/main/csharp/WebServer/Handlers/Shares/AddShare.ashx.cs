using System;
using System.Web;
using Mappers;
using Model;

namespace WebServer.Handlers.Shares
{
    /// <summary>
    /// Summary description for AddShare
    /// </summary>
    public class AddShare : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/plain";

            var shareType = context.Request.Form["type"];
            Share share;
            var user = context.Request.Form["user"];

            switch(shareType)
            {
                case "text" :
                    var text = context.Request.Form["text"];
                    share = new TextShare(user, text);
                    break;
                case "anchor" :
                    var anchor = new Uri(context.Request.Form["anchor"]);
                    share = new AnchorShare(user, anchor);
                    break;
                case "video" :
                    var video = new Uri(context.Request.Form["video"]);
                    share = new VideoShare(user, video);
                    break;
                default :
                    context.Response.Write("Invalid share type!");
                    return;
            }

            var mapper = new ShareMapper();
            mapper.Add(share);
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
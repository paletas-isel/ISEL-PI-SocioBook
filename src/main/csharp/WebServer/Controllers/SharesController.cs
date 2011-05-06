using System;
using System.Reflection;
using Mappers;
using Model;
using WebServer.Handlers.Controller;

namespace WebServer.Controllers
{
    public class SharesController : BaseController
    {
        
        public IViewResult Add(string user, string type, string content)
        {
            Type shareType = typeof (Share);
            Assembly modelAssembly = shareType.Assembly;
            string shareFullName = shareType.FullName;

            Type share;
            if ((share = modelAssembly.GetType(shareFullName.Substring(0, shareFullName.LastIndexOf(".") + 1) + type + "Share", false, true)) != null)
            {
                ConstructorInfo ctr = share.GetConstructor(new Type[2] {typeof(string), typeof (string)});
                if (ctr != null)
                {
                    Share obj = ctr.Invoke(new string[2] {user, content}) as Share;
                    ShareMapper mapper = ShareMapper.Singleton;
                    mapper.Add(obj);

                    if (obj != null) return ViewText(obj.Stamp.ToString());
                }
                return ViewText("Invalid share type!");
            }
            return ViewText("Invalid share type!");
        }

        public IViewResult Remove(string user, long stamp)
        {
            return null;
        }

        public IViewResult Get(long? newestStamp, long? oldestStamp)
        {
            return null;
        }
    }
}
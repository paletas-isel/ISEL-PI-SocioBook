using System;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Mappers;
using Model;
using System.Linq;

namespace WebServerMVC.Controllers
{
    public class SharesController : Controller
    {
        [HttpPost]
        public string Add(string user, string type, string content)
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
                    UserMapper mapperUser = UserMapper.Singleton;
                    mapper.Add(mapperUser.Get(user), obj);

                    if (obj != null) return obj.Stamp.ToString();
                }
                return "Invalid share type!!";
            }
            return "Invalid share type!";
        }

        [HttpPost]
        public void Remove(string user, long stamp)
        {
            ShareMapper mapper = ShareMapper.Singleton;
            UserMapper mapperUser = UserMapper.Singleton;

            User userO = mapperUser.Get(user);
            mapper.Remove(userO, mapper.Get(userO, stamp));
        }

        public ActionResult Get(string user, long? newestStamp, long? oldestStamp)
        {
            ShareMapper mapper = ShareMapper.Singleton; 
            UserMapper mapperUser = UserMapper.Singleton;

            User userO = mapperUser.Get(user);
            List<Share> allShares;
            if (!newestStamp.HasValue)
            {
                allShares = new List<Share>(mapper.GetAll(userO));
            }
            else
            {
                allShares = new List<Share>(mapper.GetAllAfterStamp(userO, newestStamp.Value));

                if(oldestStamp.HasValue)
                {
                    allShares.AddRange(mapper.GetAllStampMissingBetween(userO, newestStamp.Value, oldestStamp.Value).Select(p => new TextShare("delete", "delete") { Stamp = p }));
                }
            }
            return Json(allShares.ToArray());
        }
    }
}
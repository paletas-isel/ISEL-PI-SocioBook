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
        public JsonResult Add(string user, string type, string content)
        {
            var writer = User.Identity.Name;
            
            Type shareType = typeof (Share);
            Assembly modelAssembly = shareType.Assembly;
            string shareFullName = shareType.FullName;

            Type share;
            if ((share = modelAssembly.GetType(shareFullName.Substring(0, shareFullName.LastIndexOf(".") + 1) + type + "Share", false, true)) != null)
            {
                ConstructorInfo ctr = share.GetConstructor(new Type[2] {typeof(string), typeof (string)});
                if (ctr != null)
                {
                    Share obj = ctr.Invoke(new string[2] {writer, content}) as Share;
                    ShareMapper mapper = ShareMapper.Singleton;
                    UserMapper mapperUser = UserMapper.Singleton;
                    mapper.Add(mapperUser.Get(user), obj);

                    if (obj != null) return Json(new {user = writer, stamp = obj.Stamp.ToString()});
                }
                return Json(new {error = "Invalid share type!"});
            }
            return Json(new { error = "Invalid share type!" });
        }

        [HttpPost]
        public void Remove(string user, long stamp)
        {
            if ((user == null || user.Equals("")) && User.Identity.IsAuthenticated)
                user = User.Identity.Name;
            
            ShareMapper mapper = ShareMapper.Singleton;
            UserMapper mapperUser = UserMapper.Singleton;

            User userO = mapperUser.Get(user);
            mapper.Remove(userO, mapper.Get(userO, stamp));
        }

        public ActionResult Get(string user, long? newestStamp, long? oldestStamp)
        {
            if ((user == null || user.Equals("")) && User.Identity.IsAuthenticated)
                user = User.Identity.Name;
            
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
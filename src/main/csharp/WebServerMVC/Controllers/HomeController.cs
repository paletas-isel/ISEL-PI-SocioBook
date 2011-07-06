using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Mappers;
using Model;

namespace WebServerMVC.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Wall(string user)
        {
            UserMapper userMapper = UserMapper.Singleton;
            ShareMapper shareMapper = ShareMapper.Singleton;

            if (user == null && User.Identity.IsAuthenticated)
                return RedirectToAction("Wall", new {user = User.Identity.Name});
            User userO = userMapper.Get(user);

            if (userO == null)
                return RedirectToAction("LogOut", "Account");

            IEnumerable<Share> allShares = shareMapper.GetAll(userO).OrderByDescending(share => share.Stamp);
            
            return View(allShares);
        }
    }
}
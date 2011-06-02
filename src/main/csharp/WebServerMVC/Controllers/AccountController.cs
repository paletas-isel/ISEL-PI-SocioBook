using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Mappers;

namespace WebServerMVC.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult Index()
        {
            return View("LogOn");
        }

        public ActionResult LogOn(string returnUrl)
        {
            return View((object)returnUrl);
        }

        [HttpPost]
        public ActionResult LogOn(string username, string password, string returnUrl)
        {
            if(username != null && password != null)
            {
                UserMapper mapper = UserMapper.Singleton;
                var user = mapper.Get(username);

                if(user != null && user.Password.Equals(password))
                {
                    AuthModule.AuthModule.CreateCookie(System.Web.HttpContext.Current.Response, username);
                    if(Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Wall", "Home");
                }
            }

            return RedirectToAction("LogOn");
        }

        public ActionResult Register(string returnUrl)
        {
            return View((object)returnUrl);
        }

        [HttpPost]
        public ActionResult Register(string username, string password, string returnUrl)
        {
            if (username != null && password != null)
            {
                UserMapper mapper = UserMapper.Singleton;
                var user = mapper.Get(username);

                if (user != null && user.Password.Equals(password))
                {
                    AuthModule.AuthModule.CreateCookie(System.Web.HttpContext.Current.Response, username);
                    if (Url.IsLocalUrl(returnUrl))
                        return Redirect(returnUrl);
                    return RedirectToAction("Wall", "Home");
                }
            }

            return RedirectToAction("LogOn");
        }
    }
}

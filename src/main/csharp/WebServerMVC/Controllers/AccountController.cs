using System;
using System.Web.Mvc;
using Mappers;
using Model;
using WebServerMVC.Models;

namespace WebServerMVC.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult ViewFriends()
        {
            User user = UserMapper.Singleton.Get(User.Identity.Name);
            return View("Friends",user);
        }

        public ActionResult EditProfile()
        {
            User user = UserMapper.Singleton.Get(User.Identity.Name);
            return View("EditProfile", user);
        }

        [HttpPost]
        public ActionResult EditProfile(ViewUser userr)
        {
            User user = new User(userr.Username, userr.Password, userr.Name);
            UserMapper mapper = UserMapper.Singleton;
            mapper.Remove(user);
            mapper.Add(user);

            return RedirectToAction("ViewProfile");
        }

        public ActionResult ViewProfile(String userName)
        {
            User user;
            var mapper = UserMapper.Singleton;
            if (userName != null)
            {
                user = mapper.Get(userName);

                if(user == null)
                {
                    ViewBag.Error = "Profile not found!";
                    return View();
                }

                return View(user);
            }
            else
            {
                user = mapper.Get(User.Identity.Name);

                if (user == null)
                    return RedirectToAction("LogOut", "Account");
                return View(user);
            }
        }

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

                if(user == null)
                {
                    return RedirectToAction("Register", new { returnUrl });
                }
            }

            return RedirectToAction("LogOn", new { returnUrl });
        }

        public ActionResult Register(string returnUrl)
        {
            ViewBag.ReturnUrl = returnUrl;
            return View("CreateProfile");
        }

        [HttpPost]
        public ActionResult Register(ViewUser user, string returnUrl)
        {
            User modelUser = new User(user.Username, user.Password, user.Name);

            if (modelUser.Username != null && modelUser.Password != null)
            {
                UserMapper mapper = UserMapper.Singleton;
                if(mapper.Get(user.Username) != null)
                {
                    ViewBag.Error = "User already exists!";
                    return View("CreateProfile", (object)returnUrl);
                }

                mapper.Add(modelUser);

                return LogOn(modelUser.Username, modelUser.Password, returnUrl);
            }

            return RedirectToAction("LogOn");
        }

        [HttpPost]
        public ActionResult Unregister()
        {
            if (User.Identity.IsAuthenticated)
            {
                UserMapper mapper = UserMapper.Singleton;
                mapper.Remove(mapper.Get(User.Identity.Name));

                AuthModule.AuthModule.DeleteCookie(System.Web.HttpContext.Current.Response);
            }

            return RedirectToAction("Index", "Home");
        }

        public ActionResult LogOut()
        {
            if(User.Identity.IsAuthenticated)
            {
                AuthModule.AuthModule.DeleteCookie(System.Web.HttpContext.Current.Response);
            }

            return RedirectToAction("Index", "Home");
        }
    }
}

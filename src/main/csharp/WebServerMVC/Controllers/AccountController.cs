using System;
using System.Web.Mvc;
using Facebook.Web;
using Mappers;
using Model;
using WebServerMVC.Models;
using Facebook;

namespace WebServerMVC.Controllers
{
    public class AccountController : Controller
    {
        //
        // GET: /Account/

        public ActionResult ViewFriends()
        {
            User user = UserMapper.Singleton.Get(User.Identity.Name);
            return View("Friends", user);
        }

        [HttpPost]
        public ActionResult AddFriend(String userName)
        {
            User user = UserMapper.Singleton.Get(User.Identity.Name);
            User friend = UserMapper.Singleton.Get(userName);
            FriendsMapper.Singleton.Add(user, friend);
            return RedirectToAction("ViewProfile", "Account", new { userName = userName });
        }

        [HttpPost]
        public ActionResult RemoveFriend(String userName)
        {
            User user = UserMapper.Singleton.Get(User.Identity.Name);
            User friend = UserMapper.Singleton.Get(userName);
            FriendsMapper.Singleton.Remove(user, friend);
            return RedirectToAction("ViewFriends");
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

                if (user == null)
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

                if (user == null)
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
                if (mapper.Get(user.Username) != null)
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
            if (User.Identity.IsAuthenticated)
            {
                AuthModule.AuthModule.DeleteCookie(System.Web.HttpContext.Current.Response);
            }

            return RedirectToAction("Index", "Home");
        }

        // FACEBOOK AUTHENTICATION

        // Can't find a way better to do this redirects : X

        private const string logoffUrl = "/Home/Index";
        private const string redirectUrl = "/Account/OAuthFacebook";

        //
        // GET: /Account/LogOnFacebook/

        public ActionResult LogOnFacebook(string returnUrl)
        {
            FacebookOAuthClient oAuthClient = new FacebookOAuthClient(FacebookApplication.Current);
            oAuthClient.RedirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority) + redirectUrl);
            var loginUri = oAuthClient.GetLoginUrl();

            return Redirect(loginUri.AbsoluteUri);
        }

        //
        // GET: /Account/OAuthFacebook/

        public ActionResult OAuthFacebook(string code, string state)
        {
            FacebookOAuthResult oauth;

            // Need to use TryParse method because FacebookOAuthResult cannot be instantiated .
            // Ugly code that doesn't use MVC features but it is _really_ necessary ! Without that
            // I can't check if the FacebookOAuthResult is done with success.

            if (FacebookOAuthResult.TryParse(Request.Url, out oauth))
            {
                if (oauth.IsSuccess)
                {
                    var oauthClient = new FacebookOAuthClient(FacebookApplication.Current);
                    oauthClient.RedirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority) + redirectUrl);

                    dynamic resultToken = oauthClient.ExchangeCodeForAccessToken(code);

                    string accessToken = resultToken.access_token;

                    // Gather all Facebook information

                    FacebookClient fbClient = new FacebookClient(accessToken);
                    dynamic me = fbClient.Get("me");

                    long id = Convert.ToInt64(me.id);
                    string name = me.name;

                    User user = new User(id, accessToken, name);

                    if (! UserMapper.Singleton.Exists(user))
                        UserMapper.Singleton.Add(user);

                    // FacebookWebContext.Current.SignedRequest.Expires
                    AuthModule.AuthModule.CreateCookie(System.Web.HttpContext.Current.Response, user.Username);

                    // Prevents open redirection attack ! Yeah !

                    if (Url.IsLocalUrl(oauthClient.RedirectUri.ToString()))
                    {
                        return RedirectToAction("Wall", "Home");
                    }

                    return RedirectToAction("Index", "Home");

                }
            }
            return RedirectToAction("Index", "Home");
        }

        //
        // GET: /Account/LogOffFacebook/

        public ActionResult LogOffFacebook()
        {
            FacebookOAuthClient oauth = new FacebookOAuthClient();
            oauth.RedirectUri = new Uri(Request.Url.GetLeftPart(UriPartial.Authority) + logoffUrl);
            var logoutUrl = oauth.GetLogoutUrl();

            return Redirect(logoutUrl.AbsoluteUri);
        }

    }
}

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Security.Principal;
using System.Text;
using System.Web;
using System.Web.Security;
using Mappers;

namespace AuthModule
{
    public class AuthModule : IHttpModule
    {
        #region Implementation of IHttpModule

        public void Init(HttpApplication context)
        {
            context.AuthenticateRequest += (o, e) => OnAuthenticateRequest(context);

            context.EndRequest += (o, e) => OnEndRequest(context);
        }

        public void Dispose()
        {
            //throw new NotImplementedException();
        }

        #endregion

        private void OnEndRequest(HttpApplication context)
        {
            if(context.Response.StatusCode == 401)
            {
                string url = ConfigurationManager.AppSettings["LogOnPage"];
                
                context.Response.Redirect(url);
            }
        }

        private void OnAuthenticateRequest(HttpApplication context)
        {
            var cookie = context.Request.Cookies[ConfigurationManager.AppSettings["AuthCookieName"]];
            if(cookie != null && cookie["token"] != null)
            {
                bool isValidToken = FormsAuthentication.Decrypt(cookie["token"]).Name.Equals(cookie["username"]);

                if (isValidToken)
                    context.Context.User = new GenericPrincipal(new GenericIdentity(cookie["username"]), new string[0]);
            }
        }

        public static void CreateCookie(HttpResponse response, string username)
        {
            var cookie = response.Cookies[ConfigurationManager.AppSettings["AuthCookieName"]];

            if (cookie != null)
            {
                cookie["username"] = username;

                var token = new FormsAuthenticationTicket(username, true, 24 * 60);
                
                cookie["token"] = FormsAuthentication.Encrypt(token);

                cookie.Expires = DateTime.Now.AddDays(1);
            }
        }

        public static void DeleteCookie(HttpResponse response)
        {
            var cookie = response.Cookies[ConfigurationManager.AppSettings["AuthCookieName"]];

            if (cookie != null) cookie.Expires = DateTime.Now.AddDays(-1);
        }
    }
}

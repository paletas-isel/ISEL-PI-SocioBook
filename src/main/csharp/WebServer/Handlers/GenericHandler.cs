using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Reflection;
using System.Web;
using WebServer.Controllers;
using WebServer.Handlers.Controller;

namespace WebServer.Handlers
{
    public class GenericHandler : IHttpHandler
    {
        private Dictionary<string, Type> _controllers;

        #region Implementation of IHttpHandler
        
        public bool IsReusable
        {
            get { return true; }
        }

        private static void MapValidPaths(IDictionary<string, Type> map)
        {
            IEnumerable<Type> allTypes = Assembly.GetAssembly(typeof(GenericHandler)).GetTypes();

            foreach (var type in allTypes)
            {
                if (type.Namespace != null &&
                    type.Namespace.Equals(ConfigurationManager.AppSettings["ControllersNamespace"]) &&
                    type.Name.EndsWith("Controller"))
                {
                    map.Add(type.Name.Substring(0, type.Name.LastIndexOf("Controller")), type);
                }
            }

            map.Add("/", typeof(HomeController));
        }

        public void ProcessRequest(HttpContext context)
        {
            if(_controllers == null)
            {
                _controllers = new Dictionary<string, Type>();
                MapValidPaths(_controllers);
            }

            string controllerPath;
            int idx;
            if (context.Request.Url.LocalPath.Length >= 2 && (idx = context.Request.Url.LocalPath.IndexOf("/", 2)) >= 2)
                controllerPath = context.Request.Url.LocalPath.Substring(1, idx - 1);
            else
                controllerPath = "/";

            if(_controllers.ContainsKey(controllerPath))
            {
                Type controller = _controllers[controllerPath];
                BaseController controllerObj = controller.GetConstructor(new Type[0]).Invoke(new object[0]) as BaseController;

                string methodName = "";

                if(context.Request.Url.LocalPath.Length >= 2)
                {
                    idx = context.Request.Url.LocalPath.IndexOf("/", 2);
                    methodName = context.Request.Url.LocalPath.Substring(idx + 1);
                }

                if (methodName.Equals(""))
                    methodName = "Index";

                var method = controller.GetMethod(methodName);
                if (method != null && (typeof(ViewResultBase).IsAssignableFrom(method.ReturnType) || typeof(IViewResult).IsAssignableFrom(method.ReturnType)))
                {
                    NameValueCollection httpParams = GetParameters(context.Request);
                    
                    if(httpParams != null)
                    {
                        var methodReturn = InvokeControllerMethod(method, httpParams, context, controllerObj);
                        ViewResultBase methodResultB = methodReturn as ViewResultBase;
                        if(methodResultB != null)
                        {
                            methodResultB.WriteContent(context.Response);
                        }
                        else
                        {
                            IViewResult methodResult = methodReturn as IViewResult;
                            if (methodResult != null)
                            {
                                context.Response.ContentType = methodResult.ContentType;
                                context.Response.Write(methodResult.GetContent());
                            }
                        }

                    }
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Unavailable Content");
                }
            }
            else
            {
                context.Response.ContentType = "text/plain";
                context.Response.Write("Unavailable Content");
            }
        }

        private static object InvokeControllerMethod(MethodInfo method, NameValueCollection httpParams, HttpContext context, BaseController controller)
        {
            ParameterInfo[] allparams = method.GetParameters();
            object[] concreteParams = new object[allparams.Length];
            int count = 0;
            foreach (var param in allparams)
            {
                bool hasValue;
                if ((hasValue = (httpParams[param.Name] != null)) || !param.ParameterType.IsValueType || Nullable.GetUnderlyingType(param.ParameterType) != null)
                {
                    if (hasValue)
                        concreteParams[count++] = Convert.ChangeType(httpParams[param.Name], param.ParameterType);
                    else
                        concreteParams[count++] = null;
                }
                else
                {
                    context.Response.ContentType = "text/plain";
                    context.Response.Write("Unavailable Content");
                }
            }

            return method.Invoke(controller, concreteParams);
        }

        private NameValueCollection GetParameters(HttpRequest httpRequest)
        {
            if (httpRequest.RequestType.Equals("GET"))
            {
                return httpRequest.QueryString;
            }
            if (httpRequest.RequestType.Equals("POST"))
            {
                return httpRequest.Form;
            }
            return null;
        }

        #endregion
    }
}
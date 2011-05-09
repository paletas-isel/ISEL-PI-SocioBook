using System;
using System.IO;
using System.Reflection;
using System.Text;
using System.Web;

namespace WebServer.Handlers.Controller
{
    public interface IViewResult
    {
        string ContentType { get; }

        string GetContent();
    }

    public abstract class ViewResultBase : IViewResult
    {
        private readonly string _contentType;
        private readonly string _content;

        protected ViewResultBase(string content, string contentType)
        {
            _content = content;
            _contentType = contentType;
        }

        #region Implementation of IViewResult

        public string ContentType
        {
            get { return _contentType; }
        }

        public string GetContent()
        {
            return _content;
        }

        #endregion

        public void WriteContent(HttpResponse response)
        {
            response.ContentType = ContentType;
            DoWriteContent(response);
        }

        protected virtual void DoWriteContent(HttpResponse response)
        {
            response.Write(GetContent());
        }
    }

    public class ViewResult : ViewResultBase
    {
        private readonly bool _isFile = true;

        public ViewResult(string fileName) : base(fileName, "text/html")
        {

        }

        public ViewResult(string fileName, params string[] param)
            : base(GenerateCompletePage(fileName, param), "text/html")
        {
            _isFile = false;
        }

        private static string GenerateCompletePage(string fileName, params string[] param)
        {
            string content;
            using (StreamReader reader = new StreamReader(HttpRuntime.AppDomainAppPath + fileName))
            {
                content = reader.ReadToEnd();
            }
            return string.Format(content, param);
        }

        #region Overrides of ViewResultBase<IntPtr>

        protected override void DoWriteContent(HttpResponse response)
        {
            if(_isFile)
                response.WriteFile(GetContent());
            else
            {
                response.Write(GetContent());
            }
        }

        #endregion
    }

    public class TextResult : ViewResultBase
    {
        public TextResult(string text) : base(text, "text/plain")
        {
            
        }
    }

    public class JsonResult<T> : ViewResultBase
    {
        public JsonResult(T obj) : base(ParseToJson(obj), "application/json")
        {

        }

        public JsonResult(T[] arrayObj)
            : base(ParseArrayToJson(arrayObj), "application/json")
        {
            
        }

        private static string ParseArrayToJson(T[] arrayObj)
        {
            StringBuilder json = new StringBuilder("[");
            
            foreach(T obj in arrayObj)
            {
                json.AppendFormat("{0}, ", ParseToJson(obj));
            }
            json.Remove(json.Length - 1, 1);
            json.Append("]");

            return json.ToString();
        }

        private static string ParseToJson(T o)
        {
            Type typeObj = o.GetType();
            StringBuilder json = new StringBuilder("{");

            foreach (var property in typeObj.GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                json.AppendFormat("{0} : {1},", property.Name, property.GetValue(o, null));
            }

            foreach (var field in typeObj.GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                json.AppendFormat("{0} : {1},", field.Name, field.GetValue(o));
            }

            json.Remove(json.Length - 1, 1);
            json.Append("}");
            return json.ToString();
        }
    }
}
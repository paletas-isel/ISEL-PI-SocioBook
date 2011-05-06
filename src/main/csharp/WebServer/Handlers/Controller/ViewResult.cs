using System;
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

        public ViewResultBase(string content, string contentType)
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

        protected abstract void DoWriteContent(HttpResponse response);
    }

    public class ViewResult : ViewResultBase
    {
        public ViewResult(string fileName) : base(fileName, "text/html")
        {

        }

        #region Overrides of ViewResultBase<IntPtr>

        protected override void DoWriteContent(HttpResponse response)
        {
            response.WriteFile(GetContent());
        }

        #endregion
    }

    public class TextResult : ViewResultBase
    {
        public TextResult(string text) : base(text, "text/plain")
        {
            
        }

        #region Overrides of ViewResultBase

        protected override void DoWriteContent(HttpResponse response)
        {
            response.Write(GetContent());
        }

        #endregion
    }
}
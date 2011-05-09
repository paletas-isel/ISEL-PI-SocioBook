namespace WebServer.Handlers.Controller
{
    public class BaseController
    {
        public ViewResult View(string fileName, params string[] param)
        {
            return new ViewResult(fileName, param);
        } 

        public TextResult ViewText(string text)
        {
            return new TextResult(text);
        }

        public JsonResult<T> ViewJson<T>(T obj)
        {
            return new JsonResult<T>(obj);
        }

        public JsonResult<T> ViewJson<T>(T[] array)
        {
            return new JsonResult<T>(array);
        }
    }
}
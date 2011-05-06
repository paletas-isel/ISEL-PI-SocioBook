namespace WebServer.Handlers.Controller
{
    public class BaseController
    {
        public ViewResult View(string fileName)
        {
            return new ViewResult(fileName);
        } 

        public TextResult ViewText(string text)
        {
            return new TextResult(text);
        }
    }
}
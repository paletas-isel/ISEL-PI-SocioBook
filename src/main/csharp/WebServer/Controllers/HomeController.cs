using WebServer.Handlers.Controller;

namespace WebServer.Controllers
{
    public class HomeController : BaseController
    {
        public IViewResult Index()
        {
            return View("\\View\\index.html");
        }
    }
}
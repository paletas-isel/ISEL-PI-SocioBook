using System.Collections.Generic;
using Mappers;
using Model;
using WebServer.Handlers.Controller;
using WebServer.View.Templates;
using System.Linq;

namespace WebServer.Controllers
{
    public class HomeController : BaseController
    {
        public IViewResult Index()
        {
            return View("\\View\\index.html");
        }

        public IViewResult Wall(string user)
        {
            UserMapper userMapper = UserMapper.Singleton;
            ShareMapper shareMapper = ShareMapper.Singleton;

            User userO = userMapper.Get(user);
            IEnumerable<Share> allShares = shareMapper.GetAll(userO).OrderByDescending(p=> p.Stamp);

            DecoratorContainer container = new DecoratorContainer();

            IEnumerable<IHtmlDecorator> decorators = allShares.Select(container.CreateInstance);

            return View("\\View\\wall.html", new DecoratorComposite(decorators.ToArray()).ToHtmlView());
        }
    }
}
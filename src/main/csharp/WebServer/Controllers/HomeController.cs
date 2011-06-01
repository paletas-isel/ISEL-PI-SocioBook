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
            IEnumerable<Share> allShares = shareMapper.GetAll(userO).OrderByDescending(share => share.Stamp);

            DecoratorContainer container = new DecoratorContainer();

            IEnumerable<IHtmlDecorator> decorators = allShares.Select(container.CreateInstance);
            long biggestStamp = -1;
            long smallestStamp = long.MaxValue;

            foreach(Share s in allShares)
            {
                if (s.Stamp > biggestStamp) biggestStamp = s.Stamp;

                if (s.Stamp < smallestStamp) smallestStamp = s.Stamp;
            }

            if(biggestStamp == -1)
            {
                biggestStamp = smallestStamp = 0;
            }

            return View("\\View\\wall.html", biggestStamp.ToString(), smallestStamp.ToString(), new DecoratorComposite(decorators.ToArray()).ToHtmlView());
        }
    }
}
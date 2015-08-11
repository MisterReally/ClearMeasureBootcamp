using ClearMeasure.Bootcamp.Core.Services;
using Microsoft.AspNet.Mvc;
using UI.Models;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    public class NavigationController : Controller
    {
        private readonly IUserSession _session;

        public NavigationController(IUserSession session)
        {
            _session = session;
        }

        // todo: target for MVC6 rework
        //[ChildActionOnly]
        public ActionResult Menu()
        {
            var currentUser = _session.GetCurrentUser();

            var model = new NavigationMenuModel();

            return PartialView(model);
        }

    }
}

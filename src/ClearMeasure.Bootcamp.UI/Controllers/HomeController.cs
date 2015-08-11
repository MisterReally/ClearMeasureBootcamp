// using ClearMeasure.Bootcamp.UI.Helpers.ActionFilters;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    // todo: target for MVC6 rework
    //[AddUserMetaDataToViewData]
    [Authorize]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
    }
}
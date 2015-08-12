// using ClearMeasure.Bootcamp.UI.Helpers.ActionFilters;

using ClearMeasure.Bootcamp.UI.Models.SelectListProviders;
using Microsoft.AspNet.Authorization;
using Microsoft.AspNet.Mvc;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    // todo: target for MVC6 rework -action filter
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

using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Services;
using Microsoft.AspNet.Mvc;
using System;

namespace ClearMeasure.Bootcamp.UI.Helpers.ActionFilters
{
    public class AddUserMetaDataToViewDataAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuted(ActionExecutedContext filterContext)
        {
            // todo: target for MVC6 rework -DependencyResolver
            throw new NotImplementedException();
            //var userSession = filterContext.HttpContext.ApplicationServices.GetService(typeof(IUserSession));

            //Employee currentUser = userSession.GetCurrentUser();
            //if (currentUser != null)
            //{
            //    filterContext.Controller.ViewData["CurrentUserName"] = currentUser.UserName;
            //    filterContext.Controller.ViewData["CurrentUserFullName"] = currentUser.GetFullName();
            //}

            //base.OnActionExecuted(filterContext);
        }
    }
}
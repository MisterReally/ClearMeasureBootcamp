using System.Collections.Generic;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.Core.Services;
using Microsoft.AspNet.Mvc.Rendering;

namespace ClearMeasure.Bootcamp.UI.Models.SelectListProviders
{
    public class UserSelectListProvider
    {
        public static IEnumerable<SelectListItem> GetOptions()
        {
            return GetOptions(null);
        }

        public static IEnumerable<SelectListItem> GetOptions(string selected)
        {
            // todo: target for MVC6 rework -DependencyResolver
            //var bus = DependencyResolver.Current.GetService<Bus>();

            var result = new List<SelectListItem>();

            var empSpec = new EmployeeSpecificationQuery();
            //Employee[] employees = bus.Send(empSpec).Results;

            //foreach (Employee employee in employees)
            //{
            //    result.Add(new SelectListItem
            //    {
            //        Text = employee.GetFullName(),
            //        Value = employee.UserName,
            //        Selected = (employee.UserName == selected)
            //    });
            //}

            return result;
        }

        public static IEnumerable<SelectListItem> GetOptionsWithBlank(string selected)
        {
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem {Text = "<Any>", Value = ""});

            result.AddRange(GetOptions(selected));

            return result;
        }
    }
}
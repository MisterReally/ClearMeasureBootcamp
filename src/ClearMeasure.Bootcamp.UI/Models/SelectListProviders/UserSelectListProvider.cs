using System.Collections.Generic;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.Core.Services;
using MediatR;
using Microsoft.AspNet.Mvc.Rendering;

namespace ClearMeasure.Bootcamp.UI.Models.SelectListProviders
{
    public class UserSelectListProvider
    {
        private readonly IMediator _bus;

        public UserSelectListProvider(IMediator bus)
        {
            _bus = bus;
        }

        public IEnumerable<SelectListItem> GetOptions()
        {
            return GetOptions(null);
        }

        public IEnumerable<SelectListItem> GetOptions(string selected)
        {
            var result = new List<SelectListItem>();

            var empSpec = new EmployeeSpecificationQuery();
            Employee[] employees = _bus.Send(empSpec).Results;

            foreach (Employee employee in employees)
            {
                result.Add(new SelectListItem
                {
                    Text = employee.GetFullName(),
                    Value = employee.UserName,
                    Selected = (employee.UserName == selected)
                });
            }

            return result;
        }

        public IEnumerable<SelectListItem> GetOptionsWithBlank(string selected)
        {
            var result = new List<SelectListItem>();
            result.Add(new SelectListItem {Text = "<Any>", Value = ""});

            result.AddRange(GetOptions(selected));

            return result;
        }
    }
}
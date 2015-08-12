using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.SearchExpenseReports;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Services;
using ClearMeasure.Bootcamp.UI.Models;
using MediatR;
using Microsoft.AspNet.Mvc;

namespace ClearMeasure.Bootcamp.UI.Controllers
{
    public class ToDoController : Controller
    {
        private readonly IUserSession _session;
        private readonly IMediator _bus;

        public ToDoController(IUserSession session, IMediator bus)
        {
            _session = session;
            _bus = bus;
        }

        // todo: target for MVC6 rework - child action
        //[ChildActionOnly]
        public ActionResult Index()
        {
            var model = new ToDoModel();

            Employee currentUser = _session.GetCurrentUser();
            var submittedSpecification = new ExpenseReportSpecificationQuery
            {
                Approver = currentUser,
                Status = ExpenseReportStatus.Submitted
            };
            ExpenseReport[] submitted = _bus.Send(submittedSpecification).Results;
            model.Submitted = submitted;

            var approvedSpecification = new ExpenseReportSpecificationQuery
            {
                Approver = currentUser,
                Status = ExpenseReportStatus.Approved
            };
            ExpenseReport[] approved = _bus.Send(approvedSpecification).Results;
            model.Approved = approved;

            return PartialView(model);
        }

    }
}

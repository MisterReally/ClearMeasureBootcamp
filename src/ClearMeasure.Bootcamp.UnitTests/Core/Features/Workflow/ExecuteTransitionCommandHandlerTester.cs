using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Features.Workflow
{

    public class ExecuteTransitionCommandHandlerTester
    {
        [Fact(Skip = "refactor needed")]
        public void ShouldExecuteDraftTransition()
        {
            var report = new ExpenseReport();
            report.Number = "123";
            report.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            report.Submitter = employee;
            report.Approver = employee;

            var handler = new ExecuteTransitionCommandHandler(null);
            ExecuteTransitionResult result = handler.Handle(
                new ExecuteTransitionCommand(report, "Save", employee, new DateTime()));

            Assert.Equal(result.NewStatus,"Drafting");
        }
    }

}
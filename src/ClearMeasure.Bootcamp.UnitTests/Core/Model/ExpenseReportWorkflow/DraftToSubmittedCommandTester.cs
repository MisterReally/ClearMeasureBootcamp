using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{

    public class DraftToSubmittedCommandTester : StateCommandBaseTester
    {
        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new DraftToSubmittedCommand();
        }

        [Fact]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftToSubmittedCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Approver = employee;

            var command = new DraftToSubmittedCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;

            var command = new DraftToSubmittedCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        //[Test]
        //public void ShouldTransitionStateProperly()
        //{
        //    var order = new ExpenseReport();
        //    order.Number = "123";
        //    order.Status = ExpenseReportStatus.Draft;
        //    var employee = new Employee();
        //    order.Approver = employee;

        //    var command = new DraftToSubmittedCommand();
        //    command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

        //    Assert.That(order.Status, Is.EqualTo(ExpenseReportStatus.Submitted));
        //}

        //[Test]
        //public void ShouldSetFirstSubmittedOnFirstSubmission()
        //{
        //    var order = new ExpenseReport();
        //    order.Number = "123";
        //    order.Status = ExpenseReportStatus.Draft;
        //    var employee = new Employee();
        //    order.Approver = employee;

        //    var firstSubmitDate = new DateTime();

        //    var command = new DraftToSubmittedCommand();
        //    command.Execute(new ExecuteTransitionCommand(order, null, employee, firstSubmitDate));

        //    Assert.That(order.FirstSubmitted, Is.EqualTo(firstSubmitDate));
        //    Assert.That(order.LastSubmitted, Is.EqualTo(firstSubmitDate));
        //}

        [Fact]
        public void ShouldSetLastSubmittedOnEachSubmission()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Approver = employee;

            var firstSubmitDate = new DateTime(2015, 01, 01);

            var command = new DraftToSubmittedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, firstSubmitDate));

            Assert.Equal(order.FirstSubmitted, firstSubmitDate);
            Assert.Equal(order.LastSubmitted, firstSubmitDate);

            var secondSubmitDate = new DateTime(2015, 02, 02);

            var command2 = new DraftToSubmittedCommand();
            command2.Execute(new ExecuteTransitionCommand(order, null, employee, secondSubmitDate));

            Assert.Equal(order.FirstSubmitted, firstSubmitDate);
            Assert.NotEqual(order.LastSubmitted, firstSubmitDate);
            Assert.Equal(order.LastSubmitted, secondSubmitDate);

            var thirdSubmitDate = new DateTime(2015, 03, 03);

            var command3 = new DraftToSubmittedCommand();
            command3.Execute(new ExecuteTransitionCommand(order, null, employee, thirdSubmitDate));

            Assert.Equal(order.FirstSubmitted, firstSubmitDate);
            Assert.NotEqual(order.LastSubmitted, firstSubmitDate);
            Assert.NotEqual(order.LastSubmitted, secondSubmitDate);
            Assert.Equal(order.LastSubmitted, thirdSubmitDate);


        }
    }
}
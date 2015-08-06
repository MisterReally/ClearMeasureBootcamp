using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using ClearMeasure.Bootcamp.Core.Services;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{

    public class ApprovedToCancelledCommandTester : StateCommandBaseTester
    {
        [Fact]
        public void ShouldSetLastWithdrawnAndCancelledTime()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Approver = employee;

            var cancelledDate = new DateTime(2015, 01, 01);

            var command = new ApprovedToCancelledCommand();

            command.Execute(new ExecuteTransitionCommand(order, null, employee, cancelledDate));

            Assert.Equal(order.LastCancelled, cancelledDate);
            Assert.Equal(order.LastWithdrawn, cancelledDate);

            var cancelledDate2 = new DateTime(2015, 02, 02);

            var command2 = new ApprovedToCancelledCommand();
            command2.Execute(new ExecuteTransitionCommand(order, null, employee, cancelledDate2));

            Assert.NotEqual(order.LastCancelled, cancelledDate);
            Assert.Equal(order.LastCancelled, cancelledDate2);
        }
        

        [Fact]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new ApprovedToCancelledCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand{Report = order, CurrentUser = employee}));
        }

        [Fact]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            var differentEmployee = new Employee();
            var approver = new Employee();
            order.Submitter = employee;
            order.Approver = approver;

            var command = new ApprovedToCancelledCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand { Report = order, CurrentUser = approver }));

            var command2 = new ApprovedToCancelledCommand();
            Assert.False(command2.IsValid(new ExecuteTransitionCommand { Report = order, CurrentUser = differentEmployee }));
        }

        [Fact]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new ApprovedToCancelledCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand { Report = order, CurrentUser = employee }));
        }

        [Fact]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new ApprovedToCancelledCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.Equal(order.Status, ExpenseReportStatus.Cancelled);
        }

        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new ApprovedToCancelledCommand();
        }
    }
}
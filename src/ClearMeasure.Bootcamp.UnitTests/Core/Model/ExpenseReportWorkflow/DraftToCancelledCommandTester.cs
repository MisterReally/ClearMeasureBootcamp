using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using Xunit;
///using NUnit.Framework;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{
    public class DraftToCancelledCommandTester : StateCommandBaseTester
    {
        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new DraftToCancelledCommand();
        }

        [Fact]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftToCancelledCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Approver = employee;

            var command = new DraftToCancelledCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldSetLastCancelledOnExecute()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Cancelled;
            DateTime cancelledDate = new DateTime(2015, 6, 30);
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftToCancelledCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, cancelledDate));
            Assert.Equal(order.LastCancelled, cancelledDate);
        }

        [Fact]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Cancelled;
            var employee = new Employee();
            order.Approver = employee;

            var command = new DraftToCancelledCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Approver = employee;

            var command = new DraftToCancelledCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.Equal(order.Status, ExpenseReportStatus.Cancelled);
        }
    }
}

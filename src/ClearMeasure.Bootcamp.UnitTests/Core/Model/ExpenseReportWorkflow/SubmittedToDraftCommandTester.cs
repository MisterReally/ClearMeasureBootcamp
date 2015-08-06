using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using ClearMeasure.Bootcamp.Core.Services;
using Xunit;
using Rhino.Mocks;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{
    public class SubmittedToDraftCommandTester : StateCommandBaseTester
    {
        [Fact]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new SubmittedToDraftCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));

        }

        [Fact]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            var differentEmployee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToDraftCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new SubmittedToDraftCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new SubmittedToDraftCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.Equal(order.Status, ExpenseReportStatus.Draft);
            
        }

        [Fact]
        public void ShouldSetLastWithdrawnOnEachWithdraw()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var withdrawnDate = new DateTime(2015, 01, 01);

            var command = new SubmittedToDraftCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, withdrawnDate));

            Assert.Equal(order.LastWithdrawn, withdrawnDate);

            var withdrawnDate2 = new DateTime(2015, 02, 02);

            var command2 = new SubmittedToDraftCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, withdrawnDate2));

            Assert.NotEqual(order.LastWithdrawn, withdrawnDate);
            Assert.Equal(order.LastWithdrawn, withdrawnDate2);
        }

        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new SubmittedToDraftCommand();
        }
    }
}

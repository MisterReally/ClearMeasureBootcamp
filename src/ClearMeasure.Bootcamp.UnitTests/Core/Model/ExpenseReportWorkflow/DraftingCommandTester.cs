using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{

    public class DraftingCommandTester : StateCommandBaseTester
    {
        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new DraftingCommand();
        }

        [Fact]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, new Employee(), new DateTime())));
        }

        [Fact]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.Equal(order.Status, ExpenseReportStatus.Draft);
        }

    }
}
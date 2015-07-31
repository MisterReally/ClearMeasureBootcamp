using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using ClearMeasure.Bootcamp.Core.Services;
using NUnit.Framework;
using Rhino.Mocks;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{
    [TestFixture]
    public class DraftingCommandTester : StateCommandBaseTester
    {
        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new DraftingCommand();
        }

        [Test]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.True);
        }

        [Test]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, new Employee(), new DateTime())), Is.False);
        }

        [Test]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new DraftingCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.That(order.Status, Is.EqualTo(ExpenseReportStatus.Draft));
        }

    }
}
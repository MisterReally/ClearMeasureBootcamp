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
    public class SubmittedToDraftCommandTester : StateCommandBaseTester
    {
        [Test]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new SubmittedToDraftCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            var differentEmployee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToDraftCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new SubmittedToDraftCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.True);
        }

        [Test]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new SubmittedToDraftCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.That(order.Status, Is.EqualTo(ExpenseReportStatus.Draft));
        }

        [Test]
        public void ShouldSetLastWithdrawnOnEachWithdraw()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Submitter = employee;

            var withdrawnDate = new DateTime(2015,01,01);

            var command = new SubmittedToDraftCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, withdrawnDate));

            Assert.That(order.LastWithdrawn, Is.EqualTo(withdrawnDate));

            var withdrawnDate2 = new DateTime(2015, 02, 02);

            var command2 = new SubmittedToDraftCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, withdrawnDate2));

            Assert.That(order.LastWithdrawn, Is.Not.EqualTo(withdrawnDate));
            Assert.That(order.LastWithdrawn, Is.EqualTo(withdrawnDate2));
        }
        
        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new SubmittedToDraftCommand();
        }
    }
}

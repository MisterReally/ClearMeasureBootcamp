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
    public class ApprovedToSubmittedCommandTester : StateCommandBaseTester
    {
        [Test]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new ApprovedToSubmittedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            var differentEmployee = new Employee();
            order.Approver = employee;

            var command = new ApprovedToSubmittedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, differentEmployee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Approver = employee;

            var command = new ApprovedToSubmittedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.True);
        }

        [Test]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Approver = employee;

            var command = new ApprovedToSubmittedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.That(order.Status, Is.EqualTo(ExpenseReportStatus.Submitted));
        }

        [Test]
        public void ShouldPopulateLastDeclinedEachTime()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Approver = employee;

            var declineDate = new DateTime(2015,01,01);

            var command = new ApprovedToSubmittedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, declineDate));

            Assert.That(order.LastDeclined, Is.EqualTo(declineDate));

            var declineDate2 = new DateTime(2015, 02, 02);

            var command2 = new ApprovedToSubmittedCommand();
            command2.Execute(new ExecuteTransitionCommand(order, null, employee, declineDate2));

            Assert.That(order.LastDeclined, Is.Not.EqualTo(declineDate));
            Assert.That(order.LastDeclined, Is.EqualTo(declineDate2));
        }

        [Test]
        public void AssistantShouldDecline()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Approved;
            var manager = new Manager();
            var assistant = new Employee();
            order.Approver = manager;
            manager.AdminAssistant = assistant;
            

            var command = new ApprovedToSubmittedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, assistant, new DateTime())), Is.True);
        }

        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new ApprovedToSubmittedCommand();
        }
    }
}
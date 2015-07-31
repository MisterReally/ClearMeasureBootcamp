using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using NUnit.Framework;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{
    [TestFixture]
    public class SubmittedToApprovedCommandTester : StateCommandBaseTester
    {
        [Test]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToApprovedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            var approver = new Employee();
            order.Approver = approver;

            var command = new SubmittedToApprovedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldNotBeValidWithWrongApprover()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;
            var differentEmployee = new Employee();
           
            var command = new SubmittedToApprovedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, differentEmployee, new DateTime())), Is.False);
        }

        [Test]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToApprovedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())), Is.True);
        }

        [Test]
        public void ShouldBeValidWithOnBehalfApprover()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var manager = new Manager();
            var assistant = new Employee();
            manager.AdminAssistant = assistant;
            order.Approver = manager;

            var command = new SubmittedToApprovedCommand();
            Assert.That(command.IsValid(new ExecuteTransitionCommand(order, null, assistant, new DateTime())), Is.True);
        }

        [Test]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToApprovedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.That(order.Status, Is.EqualTo(ExpenseReportStatus.Approved));
        }

        [Test]
        public void ShouldSetLastApprovedEachTime()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;

            var approvedDate = new DateTime(2015,01,01);

            var command = new SubmittedToApprovedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, approvedDate));

            Assert.That(order.LastApproved, Is.EqualTo(approvedDate));

            var approvedDate2 = new DateTime(2015, 02, 02);

            var command2 = new SubmittedToApprovedCommand();
            command2.Execute(new ExecuteTransitionCommand(order, null, employee, approvedDate2));

            Assert.That(order.LastApproved, Is.Not.EqualTo(approvedDate));
            Assert.That(order.LastApproved, Is.EqualTo(approvedDate2));
        }
        
        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new SubmittedToApprovedCommand();
        }
    }
}
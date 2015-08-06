using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{

    public class SubmittedToApprovedCommandTester : StateCommandBaseTester
    {
        [Fact]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Draft;
            var employee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToApprovedCommand();

            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            var approver = new Employee();
            order.Approver = approver;

            var command = new SubmittedToApprovedCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidWithWrongApprover()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;
            var differentEmployee = new Employee();

            var command = new SubmittedToApprovedCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, differentEmployee, new DateTime())));
        }

        [Fact]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToApprovedCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldBeValidWithOnBehalfApprover()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Submitted;
            var manager = new Manager();
            var assistant = new Employee();
            manager.AdminAssistant = assistant;
            order.Approver = manager;

            var command = new SubmittedToApprovedCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, assistant, new DateTime())));
        }

        [Fact]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;

            var command = new SubmittedToApprovedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));
            Assert.Equal(order.Status, ExpenseReportStatus.Approved);
        }

        [Fact]
        public void ShouldSetLastApprovedEachTime()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Submitted;
            var employee = new Employee();
            order.Approver = employee;

            var approvedDate = new DateTime(2015, 01, 01);

            var command = new SubmittedToApprovedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, approvedDate));

            Assert.Equal(order.LastApproved, approvedDate);

            var approvedDate2 = new DateTime(2015, 02, 02);

            var command2 = new SubmittedToApprovedCommand();
            command2.Execute(new ExecuteTransitionCommand(order, null, employee, approvedDate2));

            Assert.NotEqual(order.LastApproved, approvedDate);
            Assert.Equal(order.LastApproved, approvedDate2);
        }

        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new SubmittedToApprovedCommand();
        }
    }
}
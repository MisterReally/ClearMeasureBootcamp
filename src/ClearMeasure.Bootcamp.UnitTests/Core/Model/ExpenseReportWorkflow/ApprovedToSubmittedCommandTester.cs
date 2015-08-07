using System;
using ClearMeasure.Bootcamp.Core.Features.Workflow;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportWorkflow;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model.ExpenseReportWorkflow
{

    public class ApprovedToSubmittedCommandTester : StateCommandBaseTester
    {
        [Fact]
        public void ShouldNotBeValidInWrongStatus()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Submitter = employee;

            var command = new ApprovedToSubmittedCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldNotBeValidWithWrongEmployee()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            var differentEmployee = new Employee();
            order.Approver = employee;

            var command = new ApprovedToSubmittedCommand();
            Assert.False(command.IsValid(new ExecuteTransitionCommand(order, null, differentEmployee, new DateTime())));
        }

        [Fact]
        public void ShouldBeValid()
        {
            var order = new ExpenseReport();
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Approver = employee;

            var command = new ApprovedToSubmittedCommand();
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, employee, new DateTime())));
        }

        [Fact]
        public void ShouldTransitionStateProperly()
        {
            var order = new ExpenseReport();
            order.Number = "123";
            order.Status = ExpenseReportStatus.Approved;
            var employee = new Employee();
            order.Approver = employee;

            var command = new ApprovedToSubmittedCommand();
            command.Execute(new ExecuteTransitionCommand(order, null, employee, new DateTime()));

            Assert.Equal(order.Status, ExpenseReportStatus.Submitted);
        }

        [Fact]
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

            Assert.Equal(order.LastDeclined, declineDate);

            var declineDate2 = new DateTime(2015, 02, 02);

            var command2 = new ApprovedToSubmittedCommand();
            command2.Execute(new ExecuteTransitionCommand(order, null, employee, declineDate2));

            Assert.NotEqual(order.LastDeclined, declineDate);
            Assert.Equal(order.LastDeclined, declineDate2);
        }

        [Fact]
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
            Assert.True(command.IsValid(new ExecuteTransitionCommand(order, null, assistant, new DateTime())));
        }

        protected override StateCommandBase GetStateCommand(ExpenseReport order, Employee employee)
        {
            return new ApprovedToSubmittedCommand();
        }
    }
}
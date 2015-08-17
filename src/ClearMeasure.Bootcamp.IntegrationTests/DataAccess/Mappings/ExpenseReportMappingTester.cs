using System;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using FluentNHibernate.Utils;
using NHibernate;
using Xunit;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{

    public class ExpenseReportMappingTester
    {
        [Fact]
        public void ShouldSaveAuditEntries()
        {
            var configPath = DatabaseTester.ResolveTestConfigPath();
            new DatabaseTester().Clean(configPath);

            var creator = new Employee("1", "1", "1", "1");
            var assignee = new Employee("2", "2", "2", "2");
            var order = new ExpenseReport();
            order.Submitter = creator;
            order.Approver = assignee;
            order.Title = "foo";
            order.Description = "bar";
            order.ChangeStatus(ExpenseReportStatus.Approved);
            order.Number = "123";
            order.AddAuditEntry(new AuditEntry(creator, DateTime.Now, ExpenseReportStatus.Submitted,
                                                  ExpenseReportStatus.Approved));

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                session.SaveOrUpdate(creator);
                session.SaveOrUpdate(assignee);
                session.SaveOrUpdate(order);
                session.Transaction.Commit();
            }

            ExpenseReport rehydratedExpenseReport;
            using (ISession session2 = DataContext.GetTransactedSession(configPath))
            {
                rehydratedExpenseReport = session2.Load<ExpenseReport>(order.Id);
            }

            var x = order.GetAuditEntries()[0];
            var y = rehydratedExpenseReport.GetAuditEntries()[0];
            Assert.Equal(x.EndStatus, y.EndStatus);

        }

        [Fact]
        public void ShouldSaveExpenses()
        {
            var configPath = DatabaseTester.ResolveTestConfigPath();
            new DatabaseTester().Clean(configPath);

            var creator = new Employee("1", "1", "1", "1");
            var assignee = new Employee("2", "2", "2", "2");
            var order = new ExpenseReport();
            order.Submitter = creator;
            order.Approver = assignee;
            order.Title = "foo";
            order.Description = "bar";
            order.ChangeStatus(ExpenseReportStatus.Approved);
            order.Number = "123";
            order.AddExpense("howdy", 123.45m);

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                session.SaveOrUpdate(creator);
                session.SaveOrUpdate(assignee);
                session.SaveOrUpdate(order);
                session.Transaction.Commit();
            }

            ExpenseReport rehydratedExpenseReport;
            using (ISession session2 = DataContext.GetTransactedSession(configPath))
            {
                rehydratedExpenseReport = session2.Load<ExpenseReport>(order.Id);
            }

            Expense x = order.GetExpenses()[0];
            Expense y = rehydratedExpenseReport.GetExpenses()[0];

            Assert.Equal(x.Description, y.Description);
            Assert.Equal(x.Amount, y.Amount);

        }

        [Fact]
        public void ShouldSaveExpenseReportWithNewProperties()
        {
            // Clean the database
            var configPath = DatabaseTester.ResolveTestConfigPath();
            new DatabaseTester().Clean(configPath);

            // Make employees
            var creator = new Employee("1", "1", "1", "1");
            var assignee = new Employee("2", "2", "2", "2");
            DateTime testTime = new DateTime(2015, 1, 1);
            // popluate ExpenseReport
            var report = new ExpenseReport
            {
                Submitter = creator,
                Approver = assignee,
                Title = "TestExpenseReport",
                Description = "This is an expense report test",
                Number = "123",
                MilesDriven = 100,
                Created = testTime,
                FirstSubmitted = testTime,
                LastSubmitted = testTime,
                LastWithdrawn = testTime,
                LastCancelled = testTime,
                LastApproved = testTime,
                LastDeclined = testTime,
                Total = 100.25m
            };

            report.ChangeStatus(ExpenseReportStatus.Approved);
            report.AddAuditEntry(new AuditEntry(creator, DateTime.Now, ExpenseReportStatus.Submitted,
                                                  ExpenseReportStatus.Approved));

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                session.SaveOrUpdate(creator);
                session.SaveOrUpdate(assignee);
                session.SaveOrUpdate(report);
                session.Transaction.Commit();
            }

            ExpenseReport pulledExpenseReport;
            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                pulledExpenseReport = session.Load<ExpenseReport>(report.Id);
            }

            Assert.Equal(pulledExpenseReport.MilesDriven, report.MilesDriven);
            Assert.Equal(pulledExpenseReport.Created, report.Created);
            Assert.Equal(pulledExpenseReport.FirstSubmitted, report.FirstSubmitted);
            Assert.Equal(pulledExpenseReport.LastSubmitted, report.LastSubmitted);
            Assert.Equal(pulledExpenseReport.LastWithdrawn, report.LastWithdrawn);
            Assert.Equal(pulledExpenseReport.LastCancelled, report.LastCancelled);
            Assert.Equal(pulledExpenseReport.LastApproved, report.LastApproved);
            Assert.Equal(pulledExpenseReport.LastDeclined, report.LastDeclined);
            Assert.Equal(pulledExpenseReport.Total, report.Total);

        }
    }
}
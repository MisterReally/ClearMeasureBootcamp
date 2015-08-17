using System;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NHibernate;
using Xunit;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{
    public class ExpenseReportFactMappingTester
    {
        [Fact]
        public void ShouldAddFact()
        {
            var configPath = DatabaseTester.ResolveTestConfigPath();
            new DatabaseTester().Clean(configPath);

            var creator = new Employee("1", "1", "1", "1");
            var assignee = new Employee("2", "2", "2", "2");
            var expenseReport = new ExpenseReport
            {
                Submitter = creator,
                Approver = assignee,
                Title = "foo",
                Description = "bar",
                Number = "123"
            };
            expenseReport.ChangeStatus(ExpenseReportStatus.Approved);
            ExpenseReportFact expenseReportFact = new ExpenseReportFact(expenseReport, new DateTime(2012, 1, 1));


            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                //session.SaveOrUpdate(expenseReport);
                session.SaveOrUpdate(expenseReportFact);
                session.Transaction.Commit();
            }

            using (ISession session = DataContext.GetTransactedSession())
            {
                var reportFact = session.Load<ExpenseReportFact>(expenseReportFact.Id);

                Assert.Equal(reportFact.Total, expenseReportFact.Total);
                Assert.Equal(reportFact.TimeStamp, expenseReportFact.TimeStamp);
                Assert.Equal(reportFact.Number, expenseReportFact.Number);
                Assert.Equal(reportFact.Status, expenseReportFact.Status);
                Assert.Equal(reportFact.Submitter, expenseReportFact.Submitter);
                Assert.Equal(reportFact.Approver, expenseReportFact.Approver);

            }
        }
    }
}
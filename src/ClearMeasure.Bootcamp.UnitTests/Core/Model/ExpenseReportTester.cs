using System;
using System.Linq;
using ClearMeasure.Bootcamp.Core.Model;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model
{
    public class ExpenseReportTester
    {
        [Fact]
        public void PropertiesShouldInitializeToProperDefaults()
        {
            var report = new ExpenseReport();

            Assert.Equal(report.Id, Guid.Empty);
            Assert.Equal(report.Title, string.Empty);
            Assert.Equal(report.Description, string.Empty);
            Assert.Equal(report.Description, string.Empty);
            Assert.Equal(report.Status, ExpenseReportStatus.Draft);
            Assert.Null(report.Number);
            Assert.Null(report.Submitter);
            Assert.Null(report.Approver);
            Assert.Equal(report.GetAuditEntries().Length, 0);
            Assert.Equal(report.Total, 0);

        }

        [Fact]
        public void ToStringShouldReturnNumber()
        {
            var order = new ExpenseReport();
            order.Number = "456";
            Assert.Equal(order.ToString(), "ExpenseReport 456");
        }

        [Fact]
        public void PropertiesShouldGetAndSetValuesProperly()
        {
            var report = new ExpenseReport();
            Guid guid = Guid.NewGuid();
            var creator = new Employee();
            var assignee = new Employee();
            DateTime auditDate = new DateTime(2000, 1, 1, 8, 0, 0);
            AuditEntry testAudit = new AuditEntry(creator, auditDate, ExpenseReportStatus.Submitted, ExpenseReportStatus.Approved);

            report.Id = guid;
            report.Title = "Title";
            report.Description = "Description";
            report.Status = ExpenseReportStatus.Approved;
            report.Number = "Number";
            report.Submitter = creator;
            report.Approver = assignee;
            report.AddAuditEntry(testAudit);

            Assert.Equal(report.Id, guid);
            Assert.Equal(report.Title, "Title");
            Assert.Equal(report.Description, "Description");
            Assert.Equal(report.Status, ExpenseReportStatus.Approved);
            Assert.Equal(report.Number, "Number");
            Assert.Equal(report.Submitter, creator);
            Assert.Equal(report.Approver, assignee);
            Assert.Equal(report.GetAuditEntries()[0].EndStatus, ExpenseReportStatus.Approved);
            Assert.Equal(report.GetAuditEntries()[0].Date, auditDate);

        }

        [Fact]
        public void ShouldShowFriendlyStatusValuesAsStrings()
        {
            var report = new ExpenseReport();
            report.Status = ExpenseReportStatus.Submitted;
            Assert.Equal(report.FriendlyStatus, "Submitted");
        }

        [Fact]
        public void ShouldChangeStatus()
        {
            var report = new ExpenseReport();
            report.Status = ExpenseReportStatus.Draft;
            report.ChangeStatus(ExpenseReportStatus.Submitted);
            Assert.Equal(report.Status, ExpenseReportStatus.Submitted);
        }

        [Fact]
        public void ShouldAddNewExpense()
        {
            var report = new ExpenseReport();
            report.Description = "TestReportDescription";
            report.Total = new decimal(97.34);

            report.AddExpense(report.Description, report.Total);

            Assert.Equal(report._expenses.Count, 1);
            Assert.Equal(report._expenses.First().Description, report.Description);
        }
    }
}
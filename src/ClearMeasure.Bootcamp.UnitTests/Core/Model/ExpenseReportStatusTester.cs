using ClearMeasure.Bootcamp.Core.Model;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model
{

    public class ExpenseReportStatusTester
    {
        [Fact]
        public void ShouldListAllStatuses()
        {
            ExpenseReportStatus[] statuses = ExpenseReportStatus.GetAllItems();

            Assert.Equal(statuses.Length, 4);
            Assert.Equal(statuses[0], ExpenseReportStatus.Draft);
            Assert.Equal(statuses[1], ExpenseReportStatus.Submitted);
            Assert.Equal(statuses[2], ExpenseReportStatus.Approved);
            Assert.Equal(statuses[3], ExpenseReportStatus.Cancelled);

        }

        [Fact]
        public void CanParseOnKey()
        {
            ExpenseReportStatus draft = ExpenseReportStatus.Parse("draft");
            Assert.Equal(draft, ExpenseReportStatus.Draft);

            ExpenseReportStatus submitted = ExpenseReportStatus.Parse("submitted");
            Assert.Equal(submitted, ExpenseReportStatus.Submitted);

        }
    }
}
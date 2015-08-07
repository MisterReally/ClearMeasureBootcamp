using System;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Services;
using ClearMeasure.Bootcamp.Core.Services.Impl;
using Rhino.Mocks;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Services
{
    
    public class ExpenseReportBuilderTester
    {
        [Fact]
        public void ShouldCorrectlyBuild()
        {
            var mocks = new MockRepository();
            var generator = mocks.StrictMock<INumberGenerator>();
            ICalendar calendar = new StubbedCalendar(new DateTime(2000, 1, 1));
            Expect.On(generator).Call(generator.GenerateNumber()).Return("124");
            mocks.ReplayAll();

            var builder = new ExpenseReportBuilder(generator, calendar);
            var creator = new Employee();
            ExpenseReport expenseReport = builder.Build(creator);

            mocks.VerifyAll();

            Assert.Equal(expenseReport.Submitter, creator);
            Assert.Equal(expenseReport.Number, "124");
            Assert.Null(expenseReport.Approver);
            Assert.Equal(expenseReport.Title, string.Empty);
            Assert.Equal(expenseReport.Description, string.Empty);
            Assert.Equal(expenseReport.Status, ExpenseReportStatus.Draft);
            Assert.Equal(expenseReport.Created, new DateTime(2000, 1, 1));

        }
    }
}
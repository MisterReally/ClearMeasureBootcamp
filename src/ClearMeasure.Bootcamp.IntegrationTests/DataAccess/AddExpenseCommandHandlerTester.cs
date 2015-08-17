using System;
using System.Collections.Generic;
using System.Linq;
using Autofac;
using ClearMeasure.Bootcamp.Core;
using ClearMeasure.Bootcamp.Core.Features.MutlipleExpenses;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.Core.Model.ExpenseReportAnalytics;
using ClearMeasure.Bootcamp.Core.Plugins.DataAccess;
using ClearMeasure.Bootcamp.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.Dnx.DependencyInjection;
using MediatR;
using Microsoft.Framework.DependencyInjection;
using Microsoft.Framework.Runtime.Infrastructure;
using NHibernate;
using StructureMap;
using Xunit;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    public class AddExpenseCommandHandlerTester
    {
        [Fact]
        public void ShouldCreateExpense()
        {
            var configPath = DatabaseTester.ResolveTestConfigPath();
            new DatabaseTester().Clean(configPath);
            var creator = new Employee("1", "1", "1", "1");
            var assignee = new Employee("2", "2", "2", "2");
            var report = new ExpenseReport
            {
                Submitter = creator,
                Approver = assignee,
                Title = "foo",
                Description = "bar",
                Number = "123"
            };
            var request = new AddExpenseCommand
            {
                Report = report,
                CurrentUser = creator,
                Amount = 100.00m,
                Description = "foo",
                CurrentDate = new DateTime(2000, 1,1 )
            };

            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                session.SaveOrUpdate(assignee);
                session.SaveOrUpdate(creator);
                session.SaveOrUpdate(report);
                session.Transaction.Commit();
            }



            var containerBuilder = new BootcampContainerBuilder(null);
            var container = containerBuilder.Build();
            
            var bus = (IMediator)container.Resolve(typeof(IMediator));
            bus.Send(request);

            ExpenseReport loadedReport;
            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                loadedReport = session.Load<ExpenseReport>(report.Id);
            }

            var expenses = loadedReport.GetExpenses().ToList();
            Assert.Equal(1, expenses.Count());
            Assert.Equal(100, expenses.First().Amount);
            Assert.Equal("foo", expenses.First().Description);

        }

    }
}

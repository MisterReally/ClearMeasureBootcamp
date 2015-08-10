using System;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using NHibernate;
using Xunit;

namespace ClearMeasure.Bootcamp.IntegrationTests
{

    public class ZDataLoader
    {
        [Fact, Trait("Category", "DataLoader")]
        public void PopulateDatabase()
        {
            new DatabaseTester().Clean();
            ISession session = DataContext.GetTransactedSession();


            //Trainer1
            var jpalermo = new Employee("jpalermo", "Jeffrey", "Palermo", "jeffrey@clear-measure.com");
            session.SaveOrUpdate(jpalermo);

            //Person 1
            
            //Person 2
            var jyeager = new Employee("jyeager", "jan", "yeager", "janscyeager@yahoo.com");
            session.SaveOrUpdate(jyeager);
            //Person 3
            var brheutan = new Employee("brheutan", "Burton", "Rheutan", "Rheutan7@Gmail.com");
            session.SaveOrUpdate(brheutan);
            
            //Person 4
            
            //Person 5

            //Person 6
            var fyulnady = new Employee("fyulnady", "Fredy", "Yulnady", "fyulnady@boongroup.com");
            session.SaveOrUpdate(fyulnady);
            
            //Person 7
            
            //Person 8
            
            //Person 9

            //Person 10

            //Person 11

            //Person 12

            //Person 13

            var hsimpson = new Employee("hsimpson", "Homer", "Simpson", "homer@simpson.com");
            session.SaveOrUpdate(hsimpson);

            foreach (ExpenseReportStatus status in ExpenseReportStatus.GetAllItems())
            {
                var order = new ExpenseReport();
                order.Number = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                order.Submitter = jpalermo;
                order.Approver = jpalermo;
                order.Status = status;
                order.Title = "Work Order starting in status " + status;
                order.Description = "Foo, foo, foo, foo " + status;
                new DateTime(2000, 1, 1, 8, 0, 0);
                order.ChangeStatus(ExpenseReportStatus.Draft);
                order.ChangeStatus(ExpenseReportStatus.Submitted);
                order.ChangeStatus(ExpenseReportStatus.Approved);

                session.SaveOrUpdate(order);
            }

            var order2 = new ExpenseReport();
            order2.Number = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            order2.Submitter = jpalermo;
            order2.Approver = jpalermo;
            order2.Status = ExpenseReportStatus.Approved;
            order2.Title = "Work Order starting in status ";
            order2.Description = "Foo, foo, foo, foo ";
            new DateTime(2000, 1, 1, 8, 0, 0);
            session.SaveOrUpdate(order2);

            session.Transaction.Commit();
            session.Dispose();
        }
    }
}
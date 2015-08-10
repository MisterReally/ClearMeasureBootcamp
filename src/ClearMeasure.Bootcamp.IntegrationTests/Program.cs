using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Runtime;
using NHibernate;

namespace ClearMeasure.Bootcamp.IntegrationTests
{
    public class Program
    {
        private readonly IApplicationEnvironment _appEnv;

        public Program(IApplicationEnvironment appEnv)
        {
            _appEnv = appEnv;
        }

        public IConfiguration Configuration { get; set; }

        public void Main(string[] args)
        {
            BuildConfiguration(args);

            var configPath = Configuration.Get("configPath");

            if(string.IsNullOrEmpty(configPath))
                configPath = $"{_appEnv.ApplicationBasePath}\\hibernate.cfg.xml";

            CleanData(configPath);
            LoadData(configPath);

        }

        private static void CleanData(string configPath)
        {
            new DatabaseTester().Clean(configPath);
        }

        private static void LoadData(string configPath)
        {
            ISession session = DataContext.GetTransactedSession(configPath);
            Console.Write("Generating data: ");

            //Trainer1
            var jpalermo = new Employee("jpalermo", "Jeffrey", "Palermo", "jeffrey@clear-measure.com");
            session.SaveOrUpdate(jpalermo);
            Console.Write(".");

            //Person 1
            var jyeager = new Employee("jyeager", "jan", "yeager", "janscyeager@yahoo.com");
            session.SaveOrUpdate(jyeager);
            Console.Write(".");
            //Person 2
            var brheutan = new Employee("brheutan", "Burton", "Rheutan", "Rheutan7@Gmail.com");
            session.SaveOrUpdate(brheutan);
            Console.Write(".");
            //Person 3
            var fyulnady = new Employee("fyulnady", "Fredy", "Yulnady", "fyulnady@boongroup.com");
            session.SaveOrUpdate(fyulnady);
            Console.Write(".");
            //Person 4
            var hsimpson = new Employee("hsimpson", "Homer", "Simpson", "homer@simpson.com");
            session.SaveOrUpdate(hsimpson);
            Console.Write(".");

            //Add Expense Reports
            foreach (ExpenseReportStatus status in ExpenseReportStatus.GetAllItems())
            {
                var order = new ExpenseReport();
                order.Number = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
                order.Submitter = jpalermo;
                order.Approver = jpalermo;
                order.Status = status;
                order.Title = "Work Order starting in status " + status;
                order.Description = "Foo, foo, foo, foo " + status;
                order.ChangeStatus(ExpenseReportStatus.Draft);
                order.ChangeStatus(ExpenseReportStatus.Submitted);
                order.ChangeStatus(ExpenseReportStatus.Approved);

                session.SaveOrUpdate(order);
                Console.Write(".");
            }

            var order2 = new ExpenseReport();
            order2.Number = Guid.NewGuid().ToString().Substring(0, 5).ToUpper();
            order2.Submitter = jpalermo;
            order2.Approver = jpalermo;
            order2.Status = ExpenseReportStatus.Approved;
            order2.Title = "Work Order starting in status ";
            order2.Description = "Foo, foo, foo, foo ";
            session.SaveOrUpdate(order2);

            Console.WriteLine("Data load complete.");

            // clean up Tx and dispose
            session.Transaction.Commit();
            session.Dispose();
        }

        private void BuildConfiguration(string[] args)
        {
            var builder = new ConfigurationBuilder(_appEnv.ApplicationBasePath)
                .AddJsonFile("config.json")
                .AddCommandLine(args);

            Configuration = builder.Build();
        }
    }
}

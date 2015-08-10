using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClearMeasure.Bootcamp.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using ClearMeasure.Bootcamp.IntegrationTests.DataAccess;
using Microsoft.Framework.Configuration;
using Microsoft.Framework.Runtime;
using NHibernate;

namespace ClearMeasure.Bootcamp.Dnx
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
            var connectionString = Configuration.Get("connection.connection_string");

            Console.WriteLine($"App base path: {_appEnv.ApplicationBasePath}");
            var configPath = $"{_appEnv.ApplicationBasePath}\\hibernate.cfg.xml";
            new DatabaseTester().Clean(configPath);
            ISession session = DataContext.GetTransactedSession(configPath);

            Console.WriteLine($"Session open: {session.IsOpen}");
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

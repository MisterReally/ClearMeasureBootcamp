using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    public class DatabaseTester
    {
        public static string ResolveTestConfigPath()
        {
            var appEnv = CallContextServiceLocator.Locator.ServiceProvider
            .GetService(typeof(IApplicationEnvironment)) as IApplicationEnvironment;
            var configPath = $"{appEnv.ApplicationBasePath}\\hibernate.cfg.xml";

            return configPath;
        }

        public void Clean(string configPath = "")
        {
            new DatabaseEmptier(DataContext.GetTransactedSession(configPath).SessionFactory).DeleteAllData();
        }
    }
}
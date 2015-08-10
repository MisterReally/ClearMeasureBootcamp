using ClearMeasure.Bootcamp.DataAccess.Mappings;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess
{
    public class DatabaseTester
    {
        public void Clean(string configPath = "")
        {
            new DatabaseEmptier(DataContext.GetTransactedSession(configPath).SessionFactory).DeleteAllData();
        }
    }
}
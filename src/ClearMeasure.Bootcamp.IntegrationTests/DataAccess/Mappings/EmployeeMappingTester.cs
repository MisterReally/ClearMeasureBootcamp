using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using NHibernate;
using Xunit;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{

    public class EmployeeMappingTester
    {
        [Fact]
        public void ShouldPersist()
        {
            var configPath = DatabaseTester.ResolveTestConfigPath();
            new DatabaseTester().Clean(configPath);

            var one = new Employee("1", "first1", "last1", "email1");
            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                session.Save(one);
                session.Transaction.Commit();
            }

            Employee rehydratedEmployee;
            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                rehydratedEmployee = session.Load<Employee>(one.Id);
            }

            Assert.Equal(one.UserName, rehydratedEmployee.UserName);
            Assert.Equal(one.FirstName, rehydratedEmployee.FirstName);
            Assert.Equal(one.LastName, rehydratedEmployee.LastName);
            Assert.Equal(one.EmailAddress, rehydratedEmployee.EmailAddress);

        }
    }
}
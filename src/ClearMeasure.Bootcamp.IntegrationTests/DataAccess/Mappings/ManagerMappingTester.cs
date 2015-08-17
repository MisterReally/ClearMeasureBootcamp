using System;
using System.Diagnostics;
using System.Threading;
using ClearMeasure.Bootcamp.Core.Model;
using ClearMeasure.Bootcamp.DataAccess.Mappings;
using Microsoft.Framework.Runtime;
using Microsoft.Framework.Runtime.Infrastructure;
using NHibernate;
using Xunit;

namespace ClearMeasure.Bootcamp.IntegrationTests.DataAccess.Mappings
{
    public class ManagerMappingTester
    {
        [Fact]
        public void ShouldPersist()
        {
            var configPath = DatabaseTester.ResolveTestConfigPath();
            new DatabaseTester().Clean(configPath);

            var one = new Manager("username", "Endurance", "Idehen", "Email");
            Employee adminAssistant = new Employee("Assistant", "Someone", "Else", "Email2");
            one.AdminAssistant = adminAssistant;
            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                session.Save(one);
                session.Save(adminAssistant);
                session.Transaction.Commit();
            }

            Manager rehydratedEmployee;
            using (ISession session = DataContext.GetTransactedSession(configPath))
            {
                rehydratedEmployee = session.Load<Manager>(one.Id);
            }

            Assert.Equal(one.UserName, rehydratedEmployee.UserName);
            Assert.Equal(one.FirstName, rehydratedEmployee.FirstName);
            Assert.Equal(one.LastName, rehydratedEmployee.LastName);
            Assert.Equal(one.EmailAddress, rehydratedEmployee.EmailAddress);
            Assert.Equal(one.AdminAssistant, rehydratedEmployee.AdminAssistant);
            
        }
    }
}
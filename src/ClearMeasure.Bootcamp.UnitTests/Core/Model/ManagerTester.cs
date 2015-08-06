using System;
using ClearMeasure.Bootcamp.Core.Model;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model
{

    public class ManagerTester
    {
        [Fact]
        public void AdminAssistantShouldBeAbleToActOnBehalf()
        {
            var employee = new Employee();
            var adminAssistant = new Employee();
            var manager = new Manager();
            manager.AdminAssistant = adminAssistant;

            Assert.True(manager.CanActOnBehalf(adminAssistant));
            Assert.True(manager.CanActOnBehalf(manager));
            Assert.False(manager.CanActOnBehalf(employee));


        }
    }
}
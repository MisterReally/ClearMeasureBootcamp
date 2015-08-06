using System;
using ClearMeasure.Bootcamp.Core.Model;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model
{
    public class EmployeeTester
    {
        [Fact]
        public void PropertiesShouldInitializeProperly()
        {
            var employee = new Employee();

            Assert.Equal(employee.Id, Guid.Empty);
            Assert.Null(employee.UserName);
            Assert.Null(employee.FirstName);
            Assert.Null(employee.LastName);
            Assert.Null(employee.EmailAddress);

        }

        [Fact]
        public void ToStringShouldReturnFullName()
        {
            var employee = new Employee("", "Joe", "Camel", "");
            Assert.Equal(employee.ToString(), "Joe Camel");
        }

        [Fact]
        public void PropertiesShouldGetAndSetProperly()
        {
            var employee = new Employee();
            Guid guid = Guid.NewGuid();

            employee.EmailAddress = "Test";
            employee.FirstName = "Bob";
            employee.Id = guid;
            employee.LastName = "Joe";
            employee.UserName = "bobjoe";

            Assert.Equal(employee.EmailAddress, "Test");
            Assert.Equal(employee.FirstName, "Bob");
            Assert.Equal(employee.Id, guid);
            Assert.Equal(employee.LastName, "Joe");
            Assert.Equal(employee.UserName, "bobjoe");

        }

        [Fact]
        public void ConstructorSetsFieldsProperly()
        {
            var employee = new Employee("bobjoe", "Bob", "Joe", "Test");

            Assert.Equal(employee.EmailAddress, "Test");
            Assert.Equal(employee.FirstName, "Bob");
            Assert.Equal(employee.LastName, "Joe");
            Assert.Equal(employee.UserName, "bobjoe");

        }

        [Fact]
        public void FullNameShouldCombineFirstAndLastName()
        {
            var employee = new Employee();

            employee.FirstName = "Bob";
            employee.LastName = "Joe";

            Assert.Equal(employee.GetFullName(), "Bob Joe");

        }

        [Fact]
        public void ShouldCompareEmployeesByLastName()
        {
            var employee1 = new Employee("", "1", "1", "");
            var employee2 = new Employee("", "1", "2", "");

            Assert.Equal(employee1.CompareTo(employee2), -1);
            Assert.Equal(employee1.CompareTo(employee1), 0);
            Assert.Equal(employee2.CompareTo(employee1), 1);
           
        }

        [Fact]
        public void ShouldCompareEmployeesByLastNameThenFirstName()
        {
            var employee1 = new Employee("", "1", "1", "");
            var employee2 = new Employee("", "2", "1", "");

            Assert.Equal(employee1.CompareTo(employee2), -1);
            Assert.Equal(employee1.CompareTo(employee1), 0);
            Assert.Equal(employee2.CompareTo(employee1), 1);
        }

        [Fact]
        public void ShouldImplementEquality()
        {
            var employee1 = new Employee();
            var employee2 = new Employee();

            Assert.NotEqual(employee1, employee2);
            Assert.NotEqual(employee2, employee1);

            employee1.Id = Guid.NewGuid();
            employee2.Id = employee1.Id;

            Assert.Equal(employee1, employee2);
            Assert.Equal(employee2, employee1);
            Assert.True(employee1 == employee2);
        }

        [Fact]
        public void ShouldActOnBehalf()
        {
            var thisEmployee = new Employee();
            Assert.True(thisEmployee.CanActOnBehalf(thisEmployee));
        }

        [Fact]
        public void ShouldNotActOnBehalf()
        {
            var thisEmployee = new Employee();
            var thatEmployee = new Employee();
            Assert.False(thisEmployee.CanActOnBehalf(thatEmployee));
        }
        public class EmployeeProxy : Employee
        {
        }
    }
}
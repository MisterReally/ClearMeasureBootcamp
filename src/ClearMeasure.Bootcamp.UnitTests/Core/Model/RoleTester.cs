using System;
using ClearMeasure.Bootcamp.Core.Model;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Model
{

    public class RoleTester
    {
        [Fact]
        public void Role_defaults_properly()
        {
            var role = new Role();

            Assert.Null(role.Name);
            Assert.Equal(role.Id, Guid.Empty);

            var role2 = new Role("roleName");

            Assert.Equal(role2.Name, "roleName");
        }
    }
}
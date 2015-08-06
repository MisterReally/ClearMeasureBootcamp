using ClearMeasure.Bootcamp.Core.Services.Impl;
using Xunit;

namespace ClearMeasure.Bootcamp.UnitTests.Core.Services
{

    public class NumberGeneratorTester
    {
        [Fact]
        public void ShouldBeFiveInLength()
        {
            var generator = new NumberGenerator();
            string number = generator.GenerateNumber();
            Assert.Equal(number.Length, 5);
        }
    }
}
using System.Collections.Generic;
using Xunit;

namespace Diffables.Tests
{
   
    public class AssumptionsTests
    {
        [Fact]
        public void CanGetItemByIndexFromList()
        {
            var list = new List<string> {"one", "two"};
            Assert.Equal("one", list[0]);
        }
    }
}

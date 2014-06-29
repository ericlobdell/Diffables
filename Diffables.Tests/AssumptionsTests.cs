using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

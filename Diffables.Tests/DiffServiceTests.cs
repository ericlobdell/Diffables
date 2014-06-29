﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Moq;
using Xunit;
using Diffables.Tests.Models;

namespace Diffables.Tests
{
    
    public class DiffServiceTests
    {
        private DiffablePerson p1;
        private DiffablePerson p2;
        private DiffService svc;
        private List<DiffablePerson> people; 

        public DiffServiceTests()
        {
            people = new List<DiffablePerson>();
            svc = new DiffService();
            p1 = new DiffablePerson
            {
                FirstName = "Eric",
                LastName = "Lobdell"
            };

            p1 = new DiffablePerson
            {
                FirstName = "Captain",
                LastName = "Crunch"
            };

            people.Add(p1);
            people.Add(p2);
        }

        [Fact]
        public void CanRegisterSingleIDiffable()
        {
            svc.Register(p1);
            Assert.Equal(svc.ItemsCount(), 1);

        }

        [Fact]
        public void CanRegisterIEnumerableOfIDiffable ()
        {
            svc.Register( people );
            Assert.Equal(svc.ItemsCount(), 2);
        }

        [Fact]
        public void RecordSnapShotCallsRecordStateOnAllItems()
        {
            var mock1 = new Mock<IDiffable>();
            mock1.Setup( m => m.RecordState() ).Verifiable();

            var mock2 = new Mock<IDiffable>();
            mock2.Setup( m => m.RecordState() ).Verifiable();

            svc.Register( mock1.Object );
            svc.Register( mock2.Object );
            svc.RecordSnapshot();
            mock1.Verify();
            mock2.Verify();
        }


    }
}

using System.Collections.Generic;
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

        [Fact]
        public void RollBackCallsRollBackOnAllItems ()
        {
            var mock1 = new Mock<IDiffable>();
            mock1.Setup( m => m.RollBack() ).Verifiable();

            var mock2 = new Mock<IDiffable>();
            mock2.Setup( m => m.RollBack() ).Verifiable();

            svc.Register( mock1.Object );
            svc.Register( mock2.Object );
            svc.RollBack();
            mock1.Verify();
            mock2.Verify();
        }

        [Fact]
        public void RollForwardCallsRollForwardOnAllItems ()
        {
            var mock1 = new Mock<IDiffable>();
            mock1.Setup( m => m.RollForward() ).Verifiable();

            var mock2 = new Mock<IDiffable>();
            mock2.Setup( m => m.RollForward() ).Verifiable();

            svc.Register( mock1.Object );
            svc.Register( mock2.Object );
            svc.RollForward();
            mock1.Verify();
            mock2.Verify();
        }

        [Fact]
        public void LoadVersionCallsLoadVersionOnAllItems ()
        {
            var mock1 = new Mock<IDiffable>();
            mock1.Setup( m => m.LoadVersion(2) ).Verifiable();

            var mock2 = new Mock<IDiffable>();
            mock2.Setup( m => m.LoadVersion(2) ).Verifiable();

            svc.Register( mock1.Object );
            svc.Register( mock2.Object );
            svc.LoadVersion(2);
            mock1.Verify();
            mock2.Verify();
        }

        [Fact]
        public void FlushCallsFlushOnAllItems ()
        {
            var mock1 = new Mock<IDiffable>();
            mock1.Setup( m => m.Flush() ).Verifiable();

            var mock2 = new Mock<IDiffable>();
            mock2.Setup( m => m.Flush() ).Verifiable();

            svc.Register( mock1.Object );
            svc.Register( mock2.Object );
            svc.Flush();
            mock1.Verify();
            mock2.Verify();
        }
    }
}

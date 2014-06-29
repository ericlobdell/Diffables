using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Diffables;
using Xunit.Sdk;

namespace Diffables.Tests
{
    public class DiffableObjectTests
    {
        private DiffablePerson person;
        public DiffableObjectTests()
        {
            person = new DiffablePerson
            {
                FirstName = "Eric",
                LastName = "Lobdell",
                Age = 39,
                Pets = new List<string> { "George", "Billy" },
                Address = new Address
                {
                    StreetName = "Main",
                    StreetNumber = 123,
                    City = "Anytown",
                    State = "MA"
                }
            };
        }
        [Fact]
        public void ImplementorsChangesAreBeingRecorded()
        {
            person.RecordState();

            Assert.Equal(person.GetPosition(), 1);

            person.FirstName = "Juan";
            person.RecordState();
            Assert.Equal( person.GetPosition(), 2 );

        }

        [Fact]
        public void OnlyUpdatedPropertiesAreBeingRecorded()
        {
            person.RecordState();

            Assert.Equal( person.GetPosition(), 1 );

            person.FirstName = "Juan";
            person.RecordState();
            var currentPosition = person.GetPosition();
            var delta = person.GetDelta(currentPosition);
            Assert.Equal( delta.Count, 1 );
        }

        [Fact]
        public void WorksWIthCollectionsAsValues()
        {
            person.RecordState();
            var delta = person.GetDelta( person.GetPosition() );
            Assert.Equal( delta.Count, 5 );

            // fails
            //person.Pets.Add( "Sharky" );
            //person.RecordState();
            //var currentPosition = person.GetPosition();
            //delta = person.GetDelta( currentPosition );
            //Assert.Equal( delta.Count, 1 );

        }

        [Fact]
        public void WorksWithNestedObjects()
        {
            
        }

        [Fact]
        public void RollBackRestoresValuesToPreviousState()
        {
            person.RecordState();
            Assert.Equal(person.FirstName, "Eric");

            person.FirstName = "Juan";
            person.RecordState();
            Assert.Equal( person.FirstName, "Juan" );

            person.RollBack();
            Assert.Equal( person.FirstName, "Eric" );

        }

        [Fact]
        public void RollBackDecrementsTheCurrentPosition ()
        {
            person.RecordState();
            Assert.Equal( person.GetPosition(), 1 );

            person.FirstName = "Juan";
            person.RecordState();
            Assert.Equal( person.GetPosition(), 2 );

            person.RollBack();
            Assert.Equal( person.GetPosition(), 1 );

        }

        [Fact]
        public void RollForwardRestoresValuesToStateOfNextPosition ()
        {
            person.RecordState();
            Assert.Equal( person.FirstName, "Eric" );

            person.FirstName = "Juan";
            person.RecordState();
            Assert.Equal( person.FirstName, "Juan" );

            person.RollBack();
            Assert.Equal( person.FirstName, "Eric" );
            person.RollForward();
            Assert.Equal( person.FirstName, "Juan" );

        }

        [Fact]
        public void RollForwardIncrementsTheCurrentPosition ()
        {
            person.RecordState();
            Assert.Equal( person.GetPosition(), 1 );

            person.FirstName = "Juan";
            person.RecordState();
            Assert.Equal( person.GetPosition(), 2 );

            person.RollBack();
            Assert.Equal( person.GetPosition(), 1 );

            person.RollForward();
            Assert.Equal( person.GetPosition(), 2 );

        }

        [Fact]
        public void LoadVersionSetsValuesToStateOfSpecifiedVersion()
        {
            person.RecordState();
            Assert.Equal( person.GetPosition(), 1 );

            person.FirstName = "Juan";
            person.Age = 20;
            person.RecordState();
            Assert.Equal( person.FirstName, "Juan" );
            Assert.Equal( person.Age, 20 );
            person.LoadVersion(1);
            Assert.Equal( person.FirstName, "Eric" );
            Assert.Equal( person.Age, 39 );

        }

        [Fact]
        public void LoadVersionSetsPositionToSpecifiedVersion ()
        {
            person.RecordState();
            Assert.Equal( person.GetPosition(), 1 );

            person.FirstName = "Juan";
            person.Age = 20;
            person.RecordState();
            Assert.Equal( person.GetPosition(), 2 );
            
            person.LoadVersion( 1 );
            Assert.Equal( person.GetPosition(), 1 );
        }

        [Fact]
        public void GetChangeCountReturnsCorrwectNumber()
        {
            person.RecordState();
            person.LastName = "Smith";
            person.RecordState();
            person.FirstName = "John";
            person.RecordState();
            var sut = person.GetChangeCount();
            Assert.Equal(person.GetChangeCount(), 3);
        }

        [Fact]
        public void HasPreviousReturnsTrueIfPreviousVersionExists()
        {
            person.RecordState();
            Assert.True(person.HasPrevious());
        }

        [Fact]
        public void HasPreviousReturnsFalseIfPreviousVersionDoesNotExists ()
        {
            Assert.False( person.HasPrevious() );
        }

        [Fact]
        public void HasNextReturnsTrueIfLaterVersionExists ()
        {
            person.RecordState();
            person.FirstName = "Juan";
            person.RecordState();
            person.RollBack();
            Assert.True(person.HasNext());
        }

        [Fact]
        public void HasNextReturnsFalseIfLaterVersionDoesNotExists ()
        {
            person.RecordState();
            Assert.False( person.HasNext() );
        }

        [Fact]
        public void FlushClearsAllDeltas()
        {
            person.RecordState();
            Assert.Equal(person.GetChangeCount(), 1);

            person.FirstName = "juan";
            person.RecordState();
            Assert.Equal( person.GetChangeCount(), 2 );

            person.Flush();
            Assert.Equal( person.GetChangeCount(), 0 );
        }

        [Fact]
        public void RecordStateDoesntAddDeltaIfNoChanges()
        {
            person.RecordState();
            Assert.Equal( person.GetChangeCount(), 1 );

            person.FirstName = "juan";
            person.RecordState();
            Assert.Equal( person.GetChangeCount(), 2 );
            person.RecordState();
            Assert.Equal( person.GetChangeCount(), 2 );

        }

    }
    
    public class DiffablePerson : DiffableObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<string> Pets { get; set; }
        public Address Address { get; set; }


    }

    public class Address
    {
        public int StreetNumber { get; set; }
        public string StreetName { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}

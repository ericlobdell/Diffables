using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Diffables;

namespace Diffables.Tests
{
    public class DiffableObjectTests
    {
        [Fact]
        public void ImplementorsChangesAreBeingRecorded()
        {
            var person = new DiffablePerson
            {
                FirstName = "Eric",
                LastName = "Lobdell",
                Age = 39
            };
            person.RecordState();

            Assert.Equal(person.GetPosition(), 1);

            person.FirstName = "Juan";
            person.RecordState();
            Assert.Equal( person.GetPosition(), 2 );

        }

        [Fact]
        public void OnlyUpdatedPropertiesAreBeingRecorded()
        {
            var person = new DiffablePerson
            {
                FirstName = "Eric",
                LastName = "Lobdell",
                Age = 39
            };
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
            var person = new DiffablePerson
            {
                FirstName = "Eric",
                LastName = "Lobdell",
                Age = 39,
                Pets = new List<string> { "George", "Billy" }
            };
            person.RecordState();
            var delta = person.GetDelta( person.GetPosition() );
            Assert.Equal( delta.Count, 4 );

            person.Pets.Add( "Sharky" );
            person.RecordState();
            var currentPosition = person.GetPosition();
            delta = person.GetDelta( currentPosition );
            Assert.Equal( delta.Count, 1 );

        }
    }

    public class DiffablePerson : DiffableObject
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }
        public List<string> Pets { get; set; }


    }
}

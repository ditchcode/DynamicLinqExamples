using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SandboxNonCore
{
    public class TestData
    {
        public readonly Person Bob = new Person {
                                Name = "Bob",
                                Id = 30,
                                Visits = 213,
                            };

        public readonly Person Michael = new Person { Id = 3, Name = "michael", Visits = 110 };

        public readonly Person Wendy = new Person { Id = 13, Name = "wendy", Visits = 97 };

        public readonly Person Glen = new Person { Id = 60, Name = "glen", Visits = 397 };

        public IQueryable<Person> Persons => new Person[] { Bob, Michael, Wendy, Glen }.AsQueryable();

    }
}

using System;
using System.Linq.Dynamic.Core.CustomTypeProviders;

namespace Sandbox
{
    [DynamicLinqType]
    public class Person
    {
        public string Name { get; set; }
        public int Id { get; set; }
        public int Visits { get; set; }

    }

}
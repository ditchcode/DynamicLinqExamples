using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Linq.Dynamic;
using static Newtonsoft.Json.JsonConvert;

namespace SandboxNonCore
{
    class Program
    {

        static void Main()
        {
            TesterUnit1();
            TesterUnit2();
            TesterUnit3();
            TesterUnit4();
            TesterUnit5();
            Console.ReadKey();

        }

        /// <summary>
        /// A traditional WHERE clause with no tricks
        /// </summary>
        static void TesterUnit1()
        {

            //Where param @0

            var data = new TestData();

            var dataSource = new BlackBoxDataSource<Person>();

            dataSource.DataSource = data.Persons;

            dataSource.Where = @"@0.Name == it.Name and it.Name == ""Bob"" and ((Id > 3 and Visits > 50) or Id < 3)";

            dataSource.WhereParameters = new
            {
                data.Bob  //@0
            };

            //should return one record, bob
            var query = dataSource.Select();

            Console.WriteLine($"\nClause: '{dataSource.Where}'");
            Console.WriteLine(SerializeObject(query));
        }

        /// <summary>
        /// A traditional WHERE clause with injected types
        /// </summary>
        static void TesterUnit2()
        {

            var data = new TestData();

            var dataSource = new BlackBoxDataSource<Person>();

            dataSource.DataSource = data.Persons;

            /*
             * the DynamicClass types generated for you custom types only have properties. Though you can have a delegate-typed property (like a Func).
             * System.Linq.DynamicExpression parsing doesn't understand that such delegate-typed properties can be called directly like a method.
             * So you have to use the delegate Invoke to activate the delegate.
             */
            dataSource.Where = @"@1.Compare.Invoke(@0.Name,it.Name) == 0 and it.Name == ""Bob"" and ((Id > 3 and Visits > 50) or Id < 3)";

            dataSource.WhereParameters = new
            {
                data.Bob,  //@0
                tester = Activator.CreateInstance(DynamicLinqCustomTypes<string>.Compare)  //@1
                .SetCompare(new Func<string, string, int>((s1, s2) => string.Compare(s1, s2, true)))
            };

            //should return one record, bob
            var query = dataSource.Select();

            Console.WriteLine($"\nClause: '{dataSource.Where}'");
            Console.WriteLine(SerializeObject(query));
        }


        /// <summary>
        /// Extreme injection completely takes over the Where clause
        /// </summary>
        static void TesterUnit3()
        {
            var data = new TestData();

            var dataSource = new BlackBoxDataSource<Person>();

            dataSource.DataSource = data.Persons;

            /*
             * the DynamicClass types generated for you custom types only have properties. Though you can have a delegate-typed property (like a Func).
             * System.Linq.DynamicExpression parsing doesn't understand that such delegate-typed properties can be called directly like a method.
             * So you have to use the delegate Invoke to activate the delegate.
             */
            dataSource.Where = @"@0.IsMatch.Invoke(it)";

            dataSource.WhereParameters = new
            {
                tester = Activator.CreateInstance(DynamicLinqCustomTypes<Person>.IsMatch)  //@0
                .SetIsMatch(new Func<Person, bool>(person =>
                {
                    return Regex.IsMatch(person.Name, "^(?i)(?:bob|bobby|will(iam|y)?|glen)$")
                    && (
                        person.Id < 3
                            || (person.Id > 25 && person.Visits > 200)
                         );
                }))
            };

            //should return two records, bob and glen
            var query = dataSource.Select();

            Console.WriteLine($"\nClause: '{dataSource.Where}'");
            Console.WriteLine(SerializeObject(query));

        }



        /// <summary>
        /// A traditional WHERE clause with injected types to support collections
        /// </summary>
        static void TesterUnit4()
        {

            var data = new TestData();

            var dataSource = new BlackBoxDataSource<Person>();

            dataSource.DataSource = data.Persons;

            /*
             * the DynamicClass types generated for you custom types only have properties. Though you can have a delegate-typed property (like a Func).
             * System.Linq.DynamicExpression parsing doesn't understand that such delegate-typed properties can be called directly like a method.
             * So you have to use the delegate Invoke to activate the delegate.
             */
            dataSource.Where = @"@1.IsMatch.Invoke(@0,it.Name) and it.Name == ""Bob"" and ((Id > 3 and Visits > 50) or Id < 3)";

            dataSource.WhereParameters = new
            {
                collection = new[] { "Bob","Nancy","Roy" },  //@0
                tester = Activator.CreateInstance(DynamicLinqCustomTypes<string>.IEnumerableIsMatch)  //@1
                .SetIEnumerableIsMatch(new Func<IEnumerable<string>, string, bool>((names, name) => names.Contains(name)))
            };

            //should return one record, bob
            var query = dataSource.Select();

            Console.WriteLine($"\nClause: '{dataSource.Where}'");
            Console.WriteLine(SerializeObject(query));
        }

        /// <summary>
        /// A traditional WHERE clause with injected types to support collections
        /// </summary>
        static void TesterUnit5()
        {

            var data = new TestData();

            var dataSource = new BlackBoxDataSource<Person>();

            dataSource.DataSource = data.Persons;

            /*
             * the DynamicClass types generated for you custom types only have properties. Though you can have a delegate-typed property (like a Func).
             * System.Linq.DynamicExpression parsing doesn't understand that such delegate-typed properties can be called directly like a method.
             * So you have to use the delegate Invoke to activate the delegate.
             */
            dataSource.Where = @"@1.IsMatch.Invoke(@0,it) and it.Name == ""Bob"" and ((Id > 3 and Visits > 50) or Id < 3)";

            dataSource.WhereParameters = new
            {
                collection = new[] { data.Bob, data.Michael, data.Glen },  //@0
                tester = Activator.CreateInstance(DynamicLinqCustomTypes<Person>.IEnumerableIsMatch)  //@1
                .SetIEnumerableIsMatch(new Func<IEnumerable<Person>, Person, bool>((persons, person) => persons.Contains(person)))
            };

            //should return one record, bob
            var query = dataSource.Select();

            Console.WriteLine($"\nClause: '{dataSource.Where}'");
            Console.WriteLine(SerializeObject(query));
        }


    }





}

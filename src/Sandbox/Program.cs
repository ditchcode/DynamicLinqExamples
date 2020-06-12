using System;
using System.Linq.Dynamic.Core;
using System.Linq.Dynamic.Core.CustomTypeProviders;
using System.Linq.Expressions;
using System.Text.RegularExpressions;
using System.ComponentModel;
using static Newtonsoft.Json.JsonConvert;

namespace Sandbox
{


    class Program
    {

        static void Main()
        {
            TypeDescriptor.AddAttributes(typeof(Regex), new DynamicLinqTypeAttribute());
            TestUnit1();
            TestUnit2();
            TestUnit3();
            Console.ReadKey();
        }

        /// <summary>
        /// Simple example of parsing a dynamic expression with custom types.
        /// </summary>
        static void TestUnit1()
        {
            var data = new TestData();

            var config = new ParsingConfig { CustomTypeProvider = new DynamicLinqTypeProvider() };

            const string exp = @"Regex.IsMatch(Name,""^(?i)bob(by)?$"") and Id > 3";

            //Note that since we do not have a data source, dynamic linq makes this first WHERE parameter the IT instance
            var p = Expression.Parameter(typeof(Person), "bob");

            var e = DynamicExpressionParser.ParseLambda(config, new ParameterExpression[] { p }, typeof(bool), exp);

            var result = e.Compile().DynamicInvoke(data.Bob);
            Console.WriteLine(SerializeObject(result));
        }

        /// <summary>
        /// Using a hack data source, simple query with no tricks
        /// </summary>
        static void TestUnit2()
        {
            var data = new TestData();

            var dataSource = new BlackBoxDataSource<Person>();

            dataSource.DataSource = data.Persons;

            dataSource.Where = @"Name == ""Bob"" and Id > 3";

            var result = dataSource.Select();

            Console.WriteLine(SerializeObject(result));
        }

        /// <summary>
        /// Using a hack data source, query with custom types
        /// </summary>
        static void TestUnit3()
        {
            var data = new TestData();

            var dataSource = new BlackBoxDataSource<Person>();

            dataSource.DataSource = data.Persons;

            dataSource.Where = @"Regex.IsMatch(Name,""^(?i)bob(by)?$"") and Id > 3";

            var config = new ParsingConfig { CustomTypeProvider = new DynamicLinqTypeProvider() };

            var result = dataSource.Select(config);

            Console.WriteLine(SerializeObject(result));
        }
    }
}

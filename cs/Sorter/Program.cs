using Microsoft.CodeAnalysis.CSharp.Scripting;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Sorter
{
    class Program
    {
        static void Main(string[] args)
        {
            var testArr = new TestStruct[]
            {
                new TestStruct(415, "Apple"),
                new TestStruct(30432, "Something something something"),
                new TestStruct(1, "Apple"),
                new TestStruct(32, "Cherry is the best"),
                new TestStruct(2, "Banana is yellow")
            };

            var orderedArr = testArr.OrderBy(t => t).ToArray().Select(t => t.ToString()).ToArray();

            Console.WriteLine(orderedArr);
        }
    }
}

using System;
using System.Collections.Generic;
using NUnit.Framework;

namespace FPandScala
{
    [TestFixture]
    public class HigherOrderFunctions
    {
        // Lets reinvent Linq predicates ... we take a function
        public static IEnumerable<Item> Filter(IEnumerable<Item> items, Func<Item, bool> predicate)
        {
            foreach (var item in items)
            {
                if (predicate(item))
                    yield return item;
            }
        }

        public static void HigherOrderFunction()
        {
            var items = new Item[] {new(1, "Foo"), new(2, "Bar"), new(3, "Baz")};

            // We can pass a lambda function
            var itemsWithB = Filter(items, item => item.Name.StartsWith("B"));

            // Functions and Actions are first class citizens - we can assign them to variables and pass them around
            Func<Item, bool> evenIdPredicate = item => item.Id % 2 == 0;
            var evenItems = Filter(items, evenIdPredicate);

            // And we can even return functions and actions from methods
            var greaterThan2Predicate = IdComparisonPredicateBuilder(2);
            var bigItems = Filter(items, greaterThan2Predicate);

            // Obviously Linq does this all better ...
        }

        public static Func<Item, bool> IdComparisonPredicateBuilder(int threshold)
        {
            return item => item.Id > threshold;
        }

        [Test]
        public void FuncsAndActionsAreStillMultiCastDelegates()
        {
            Action<Item> doAction = item => Console.WriteLine($"Item {item.Name} has been printed");
            doAction += item => Console.WriteLine($"Item {item.Name} has been printed ... Again!");

            var item = new Item(1, "Foo");

            // So what is printed?
            doAction(item);

            Func<Item, bool> itemFunc = item =>
            {
                Console.WriteLine("True was called");
                return true;
            };
            itemFunc += item =>
            {
                Console.WriteLine("False was called");
                return false;
            };

            // ? So what's the output?
            Console.WriteLine($"... and the winner is : {itemFunc(item)}");
        }
    }

    public record Item(int Id, string Name);
}

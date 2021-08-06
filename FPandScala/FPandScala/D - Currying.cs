using System;
using NUnit.Framework;

namespace FPandScala
{
    [TestFixture]
    public class Currying
    {
        // Currying is verbose and loses its impact in C#
        public Func<int, int> CurriedAdd(int a)
        {
            return b => a + b;
        }

        [Test]
        public void Main()
        {
            var sum = CurriedAdd(5)(3);
            Console.WriteLine(sum);

            var partialResult = CurriedAdd(5);
            var finalSum = partialResult(3);
            Console.WriteLine(finalSum);

            // Javascript guys love this style 'Immediately Invoked Function Expression (IIFE)', but this is more about scoping than maths "Module Pattern"
            // (function () {})()
        }
    }
}

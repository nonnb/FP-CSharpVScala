using System;
using NUnit.Framework;

namespace FPandScala
{
    [TestFixture]
    public class AvoidPassingByReference
    {
        // Out passes the caller's variable ADDRESS (by reference), and not a copy of the variable on the stack as is the usual in pass-by-value
        public static void Foo(out int myInt)
        {
            // Overwrite the caller's variable with this new value
            myInt = 5;
        }

        // Since we now use a lot of lambdas and closures, the problem with out becomes more obvious
        //public static Action Bar(ref int myInt) // C++ this is equivalent to int* or int& (but non-const). For reference types on the heap, this would be a double pointer
        //{
        //    // This is not allowed. Trying to capture a 'pass by reference' variable in a closure means that the lambda (a) can outlive the
        //    // caller's variable allocation on the stack.
        //    // Also, for `out` the requirement that myInt that myInt must be assigned a value before leaving method Bar cannot be guaranteed, 
        //    // because we don't know when the action will be invoked.
        //    Action a = () => { myInt = 99;};
        //    return a;
        //}

        [Test]
        public void Test()
        {
            int myInt = 5;
            Foo(out myInt); // C++ equivalent is &myInt
            Console.WriteLine(myInt);
        }
    }
}
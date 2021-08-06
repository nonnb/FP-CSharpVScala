using System;
using NUnit.Framework;

namespace FPandScala
{
    [TestFixture]
    public class TailCallOptimisation
    {
        static TailCallOptimisation()
        {
            // Show continual progress in testrunner, rather than buffering it
            Console.SetOut(TestContext.Progress);
        }

        [Test]
        public void InfiniteCount()
        {
            Add1(0);
        }

        public long Add1(long previous)
        {
            if (previous % 10000L == 0)
            {
                Console.WriteLine(previous);
            }
            // Infinite Loop. This crashes with a StackOverflow in .Net Core or 32 bit or Debug builds.
            // But on Net4.8 FW x64 release builds, it has Tail Call Optimisation
            // (TCO overwrites the stack, rather than continually pushing to it, preventing the overflow, but losing the true stack trace)
            return Add1(previous + 1);
        }
    }
}
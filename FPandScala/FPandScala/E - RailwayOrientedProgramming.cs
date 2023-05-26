using System;
using System.Linq;

namespace FPandScala
{
    public class RailwayOrientedProgramming
    {
        public Result ImperativeBranches(string value)
        {
            // Most of our orchestration / handler code looks like such
            var result = Validate(value);
            if (result != Result.Ok)
            {
                return Result.Failed;

                // Or Even worse
                // throw new BigNastyException("Your input isn't valid");
            }
            var anotherLocalResult = DoSomethingElse(value);
            if (anotherLocalResult != Result.Ok)
            {
                // These branches interrupt the 'flow' of code readability
                return Result.Failed;
            }
            // ... etc

            // And then finally
            return Result.Ok;
        }

        // You've probably familiar with the basic RO programming 'flow' with null conditional (elvis)
        // and conditional ternary operators or pattern matching (e.g. to avoid NRE's)
        // null is used to indicate error (yuk)
        public Result HalfRailway(string value)
        {
            // Note real FP won't use nullable types, it will use a Monad like Either or Maybe.
            return value
                    ?.Translate() // <- If value is NULL, then ALL steps until the switch are skipped
                    ?.Reverse() // ditto
                    .ToArray()
                    .FirstOrDefault()
                    switch
                    {
                        char c => Result.Ok,
                        null => Result.Failed
                    };
        }


        // With null conditionals and conditional ternary operators or pattern matching, we can do basic railway flows, e.g. using null to indicate error
        public Either<Error, Result> RailwayProgrammingWithEither(string value)
        {
            // The idea here is to have a single 'flow' of code (not stopping for branch checks and 'testing' of previous result after each step)
            // If there is a failure, the rest of the steps are short circuited
            // And then, at the end, a single check (I've used pattern matching) can be done to map both 'happy' and 'failed' branches
            return value
                    .Translate2()
                    .IfOk(s => new (s.ToUpper()))
                    .IfOk(s => new (s.Normalize()))
                switch
                {
                    var x when x.Success => new(Result.Ok),
                    _ => new (Result.Failed)
                };
        }


        
        
        
        
        
        
        
        // **** All of the below code is absolute cruft - it's just to 'prove' the sample flows above with monads and railway oriented programming
        
        
        
        public Result Validate(string value)
        {
            return Result.Failed;
        }

        public Result DoSomethingElse(string value)
        {
            return Result.Failed;
        }
    }

    public class Result
    {
        public static Result Ok = new Result();
        public static Result Failed = new Result();
    }

    public class Error
    {
    }

    public class Either<TLeft, TRight>
    {
        public TLeft Left { get; }
        public TRight Right { get; }

        public Either(TLeft left)
        {
            Left = left;
        }
        public Either(TRight right)
        {
            Right = right;
            Success = true;
        }

        public bool Success { get; private set; }
    }

    public static class Helpers
    {
        public static string Translate(this string input)
        {
            return input?.ToUpper();
        }

        public static Either<Error, string> Translate2(this string input)
        {
            // C# 9 is getting better !
            return new (input.ToUpper());

            // C#8 and previous ...
            // return new Either<Error, string>(Result.Ok);
        }

        public static Either<TLeft, TRight> IfOk<TLeft, TRight>(this Either<TLeft, TRight> previous, Func<TRight, Either<TLeft, TRight>> next)
        {
            if (previous.Success)
            {
                return next(previous.Right);
            }
            return previous;
        }
    }
}

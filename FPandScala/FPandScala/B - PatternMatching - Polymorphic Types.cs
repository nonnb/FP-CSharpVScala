using System.Drawing.Design;

namespace FPandScala
{
    public class PatternMatching
    {
        public string ImperativeNonTrivialMatchingBranches(ITool tool)
        {
            string result = null;

            // NB - this is a non-trivial pattern - it CANNOT be handled by a switch statement or a Map / Dictionary [Lookup] => Value

            if (tool is Drill drill && drill.Rpm > 10)
            {
                result = "Fast Drill"; // <<< Reassigning the variable ...
            }
            else if (tool is Hammer hammer && hammer.Whacks < 5)
            {
                result = "New Hammer"; // <<< Reassigning the variable ...
            }
            else
            {
                result = "Invalid tool";  // <<< Reassigning the variable ...
            }
            // Did we assign result in all branches, or is it possibly null?
            // Did we possibly overwrite result in more than one branch?
            // It's pretty hard to see this as 'one' activity, i.e. matching, projecting and assigning 

            return result;
        }

        public string NestedConditionalTernaryOperators(ITool tool)
        {
            var result = (tool is Drill drill && drill.Rpm > 10)
                ? "Fast Drill"
                : (tool is Hammer hammer && hammer.Whacks < 5)
                    ? "New Hammer"
                    : "Invalid tool";

            // Nested Ternaries can be difficult to read ... how to indent the nested branches etc.

            return result;
        }

        // C#9
        public string PatternMatchCs9(ITool tool)
        {
            var result = tool switch
            {
                Drill { Rpm: > 10 } => "Fast Drill",
                Hammer { Whacks: < 5 } => "New Hammer",
                _ => "Invalid tool"
            };
            return result;
        }


    }


    public interface ITool
    {
    }

    public record Drill(int Rpm) : ITool;

    public record Hammer(int Whacks) : ITool;
}
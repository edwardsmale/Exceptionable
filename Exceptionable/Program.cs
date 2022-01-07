using System;

namespace EAS.Exceptionable
{
    public class Program
    {
        public static void Main(string[] args)
        {
            {
                // A method with return type Exceptionable<T> can return a T:

                var result = MethodThatReturnsExceptionable(42);

                Assert(result == "42 is a great number.");
            }

            {
                // A method with return type Exceptionable<T> can return an exception:

                var result = MethodThatReturnsExceptionable(100);

                Assert(result.Exception.GetType() == typeof(ArgumentException));
            }

            {
                // An Exceptionable<T> can be tested for a value like this:

                var result = MethodThatReturnsExceptionable(100);

                if (result)
                {
                    Console.WriteLine("The result has a value.");
                }

                if (!result)
                {
                    Console.WriteLine("The result has no value.");
                }

                var flag = false;

                if (flag && result)
                {
                    Console.WriteLine("The flag is true and the result has a value.");
                }

                if (!flag || !result)
                {
                    Console.WriteLine("Either the flag is false or the result has no value.");
                }
            }

            {
                var exceptionable = new Exceptionable<int>(3);

                // An Exceptionable<T> can be compared to a T for equality:

                Assert(exceptionable == 3);
                Assert(3 == exceptionable);
            }

            {
                // An Exceptionable<T> can be assigned a T value:

                Exceptionable<int> exceptionable = 99;

                Assert(exceptionable == 99);
            }

            {
                // An Exceptionable<T> can be passed to a method that expects a T:

                var exceptionable = new Exceptionable<string>("A piece of text");

                var result = MethodThatTakesAString(exceptionable);

                Assert(result == "A piece of text was passed.");
            }

            {
                // A non-valued exceptionable throws an exception when implicitly converted to T:

                var exceptionable = new Exceptionable<string>(new Exception("Went wrong!!! :("));

                try
                {
                    MethodThatTakesAString(exceptionable); // Will throw
                }
                catch (Exception ex)
                {
                    Assert(ex.GetType() == typeof(InvalidOperationException));
                }
            }

            void Assert(bool condition)
            {
                if (!condition)
                {
                    throw new Exception("Assert failed.");
                }
            }
        }

        public static string MethodThatTakesAString(string str)
        {
            return str + " was passed.";
        }

        public static Exceptionable<string> MethodThatReturnsExceptionable(int number)
        {
            if (number <= 42)
            {
                return number + " is a great number.";
            }
            else
            {
                return new ArgumentException();
            }
        }
    }
}
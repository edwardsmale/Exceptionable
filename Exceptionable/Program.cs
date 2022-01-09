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

                Assert(
                    result.HasValue == true,
                    $"An {nameof(Exceptionable<int>)} containing a value has {nameof(result.HasValue)} == true."
                );

                Assert(
                    result == "42 is a great number.", 
                    nameof(MethodThatReturnsExceptionable) + " should return a value when the input is valid."
                );
            }

            {
                // A method with return type Exceptionable<T> can return an exception:

                var result = MethodThatReturnsExceptionable(-1);

                Assert(
                    result.HasException == true,
                    $"An {nameof(Exceptionable<int>)} containing a value has {nameof(result.HasException)} == true."
                );

                Assert(
                    result.Exception.GetType() == typeof(ArgumentException), 
                    nameof(MethodThatReturnsExceptionable) + " should return an exception when the input is invalid."
                );
            }

            {
                var exceptionable = new Exceptionable<int>(3);

                // An Exceptionable<T> can be compared to a T for equality:

                Assert(
                    exceptionable == 3, 
                    $"An {nameof(Exceptionable<int>)} containing a value can be compared for equality."
                );
            }

            {
                // An Exceptionable<T> can be assigned a T value:

                Exceptionable<int> exceptionable = 99;

                Assert(
                    exceptionable == 99, 
                    $"An {nameof(Exceptionable<int>)} can be assigned an int value."
                );
            }

            {
                // An Exceptionable<T> can be passed to a method that expects a T:

                var exceptionable = new Exceptionable<string>("A piece of text");

                var result = MethodThatTakesAString(exceptionable);

                Assert(
                    result == "A piece of text was passed.",
                    $"An {nameof(Exceptionable<string>)} can be passed to a method that expects a string."
                );
            }

            {
                // A non-valued exceptionable throws an exception when implicitly converted to T:

                var exceptionable = new Exceptionable<string>(new Exception("Went wrong!!! :("));

                try
                {
                    MethodThatTakesAString(exceptionable); // Will throw

                    Fail($"An {nameof(Exceptionable<string>)} containing an exception will throw when passed to a method expecting a string.");
                }
                catch (InvalidOperationException)
                {
                    // Success
                }
                catch (Exception)
                {
                    Fail($"An {nameof(Exceptionable<string>)} containing an exception will throw when passed to a method expecting a string.");
                }
            }

            {
                // Payment service with valid account ID and web service working.

                var paymentService = new PaymentServiceExample_WithExceptionable();

                var result = paymentService.TakePayment();
                
                Assert(
                    result == PaymentResultEnum.Success,
                    "With valid account ID and web service up, TakePayment() returns Success."
                );
            }

            {
                // Payment service with invalid account ID and web service working.

                var paymentService = new PaymentServiceExample_WithExceptionable(accountIdExists: false);

                var result = paymentService.TakePayment();

                Assert(
                    result == PaymentResultEnum.NoSuchAccount,
                    "With invalid account ID and web service up, TakePayment() returns NoSuchAccount."
                );
            }

            {
                // Payment service with valid account ID and web service down.

                var paymentService = new PaymentServiceExample_WithExceptionable(isWebServiceDown: true);

                var result = paymentService.TakePayment();

                Assert(
                    result == PaymentResultEnum.WebServiceCallFailed,
                    "With valid account ID and web service down, TakePayment() returns WebServiceCallFailed."
                );
            }

            void Assert(bool condition, string message)
            {
                if (!condition)
                {
                    Fail("Failure:" + message);
                }
                else
                {
                    Success("Success: " + message);
                }
            }

            void Success(string message) => Print(message, ConsoleColor.Green);
            void Fail(string message) => Print(message, ConsoleColor.Red);

            void Print(string message, ConsoleColor color)
            {
                Console.ForegroundColor = color;
                Console.WriteLine(message);
                Console.ForegroundColor = ConsoleColor.Gray;
            }
        }

        public static string MethodThatTakesAString(string str)
        {
            return str + " was passed.";
        }

        public static Exceptionable<string> MethodThatReturnsExceptionable(int positiveNumber)
        {
            if (positiveNumber > 0)
            {
                return positiveNumber + " is a great number.";
            }
            else
            {
                return new ArgumentException();
            }
        }
    }
}
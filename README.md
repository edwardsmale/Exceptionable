# Exceptionable&lt;T&gt;

### _A class containing either a value or an exception. A bit like Nullable&lt;T&gt;, but for exceptions._

EXAMPLES

### __Method with return type Exceptionable&lt;T&gt; can return a T or an exception:__


```C#
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
```

```C#
    var result = MethodThatReturnsExceptionable(42);

    Assert(result == "42 is a great number.");
```

```C#
    var result = MethodThatReturnsExceptionable(-1);

    Assert(result.Exception.GetType() == typeof(ArgumentException));
```

### __Exceptionable&lt;T&gt; can be passed to a method expecting a T:__

```C#
    var exceptionable = new Exceptionable<string>("A piece of text");

    var result = MethodThatTakesAString(exceptionable);

    Assert(result == "A piece of text was passed.");
```

### __Exceptionable&lt;T&gt; can be assigned a T:__

```C#
    Exceptionable<int> exceptionable = 99;

    Assert(exceptionable == 99);
```

### __Exceptionable&lt;T&gt; can be compared to T for equality:__

```C#
    var exceptionable = new Exceptionable<int>(3);

    Assert(exceptionable == 3);
    Assert(3 == exceptionable);
```

## Web Service Example

### __Traditional exception-based approach:__

```C#
    public class PaymentServiceExample_WithExceptions
    {
        public PaymentResultEnum TakePayment()
        {
            try
            {
                var account = this.GetAccount(12345);

                var paymentResult = this.CallWebService();

                // other code

                this.log.Info("Payment taken successfully.");

                return PaymentResultEnum.Success;
            }
            catch (WebException ex)
            {
                // Web service exception handling is a long way from code which threw the exception.
                
                // Catch blocks are in a different order to the code that caused the exception, as 
                // more-specific exceptions have to be caught first.

                // account.Name is not available in this context:

                this.log.LogError($"Payment failed.  Name = {account.Name}.", ex); // Oops!

                return PaymentResultEnum.WebServiceCallFailed;
            }
            catch (Exception ex)
            {
                // Again, the exception handling is a long way from where the exception arose.

                this.log.Error($"Payment failed.  Account ID = {accountId}.", ex);

                return PaymentResultEnum.NoSuchAccount;
            }
        }

        private Account GetAccount(int accountId)
        {
            if (no_such_account)
            {
                throw new ArgumentException("No account with ID " + accountId);
            }
            else
            {
                return new Account(accountId, "Ed Smale");
            }
        }

        private string CallWebService()
        {
            if (web_request_fails)
            {
                throw new WebException("The web request failed :(");
            }
            else
            {
                return "OK";
            }
        }
    }
```

### __Exceptionable&lt;T&gt; approach:__

```C#
    public class PaymentService
    {
        public PaymentResultEnum TakePayment()
        {
            var account = this.GetAccount(12345);

            if (account.HasException)
            {
                // Code for handling account exception is near the method call which generated the exception.
                // We know which bit of code generated the exception, so we can take the appropriate action.

                this.log.LogError($"Payment failed.  Account ID = {accountId}.", account.Exception);

                return PaymentResultEnum.NoSuchAccount;
            }

            var paymentResult = this.CallWebService();

            if (paymentResult.HasException)
            {
                // The 'account' variable is available, so we can log the account name.

                this.log.LogError($"Payment failed.  Account ID = {accountId}, Name = {account.Value.Name}.", paymentResult.Exception);

                return PaymentResultEnum.WebServiceCallFailed;
            }

            // other code

            this.log.Info("Payment taken successfully.");

            return PaymentResultEnum.Success;
        }

        // Returns an account, or an Exception in the event of failure:

        private Exceptionable<Account> GetAccount(int accountId)
        {
            if (no_such_account)
            {
                return new ArgumentException("No account with ID " + accountId);
            }
            else
            {
                return new Account(accountId, "Ed Smale");
            }
        }

        // Returns a string, or an Exception in the event of failure:

        private Exceptionable<string> CallWebService()
        {
            if (web_request_fails)
            {
                return new WebException("The web request failed :(");
            }
            else
            {
                return "OK";
            }
        }
    }
```
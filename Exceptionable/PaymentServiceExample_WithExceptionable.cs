using System;
using System.Net;

namespace EAS.Exceptionable
{
    /// <summary>
    /// Illustrates the advantages of returning <see cref="Exceptionable{T}"/> from a method instead of 
    /// throwing exceptions and catching them in the caller.
    /// 
    /// The counterpart class <see cref="PaymentServiceExample_WithExceptions"/> illustrates how the more
    /// traditional exception-based approach makes the code harder to follow.
    /// </summary>
    public class PaymentServiceExample_WithExceptionable
    {
        private readonly bool accountIdExists;
        private readonly bool isWebServiceDown;

        private readonly Log log;

        public PaymentServiceExample_WithExceptionable(bool accountIdExists = true, bool isWebServiceDown = false)
        {
            this.accountIdExists = accountIdExists;
            this.isWebServiceDown = isWebServiceDown;

            this.log = new Log();
        }

        public PaymentResultEnum TakePayment(int accountId = 12345)
        {
            var account = this.GetAccount(accountId);

            if (account.HasException)
            {
                // Code for handling account exception is near the method call which generated the exception.
                // We know which bit of code generated the exception, so we can take the appropriate action.

                this.log.Error($"Payment failed.  Account ID = {accountId}.", account.Exception);

                return PaymentResultEnum.NoSuchAccount;
            }

            var paymentResult = this.CallWebService();

            if (paymentResult.HasException)
            {
                // Code for handling account exception is near the method call which generated the exception.
                // We know which bit of code generated the exception, so we can take the appropriate action.

                // The 'account' variable is available, so we can log the account name.

                this.log.Error($"Payment failed.  Account ID = {accountId}, Name = {account.Value.Name}.", paymentResult.Exception);

                return PaymentResultEnum.WebServiceCallFailed;
            }

            // other code

            this.log.Info("Payment taken successfully.");

            return PaymentResultEnum.Success;
        }

        private Exceptionable<Account> GetAccount(int accountId)
        {
            if (!this.accountIdExists)
            {
                return new ArgumentException("No account with ID " + accountId);
            }
            else
            {
                return new Account(accountId, "Ed Smale");
            }
        }

        private Exceptionable<string> CallWebService()
        {
            if (this.isWebServiceDown)
            {
                return new WebException("The web request failed :(");
            }
            else
            {
                return "OK";
            }
        }
    }
}
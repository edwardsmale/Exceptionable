using System;
using System.Net;

namespace EAS.Exceptionable
{
    /// <summary>
    /// Illustrates the downsides of traditional, exception-based approach, where a piece of code
    /// calls a method, which may throw an exception in the event of a failure, and the calling
    /// code catching those exceptions.
    /// 
    /// The counterpart class <see cref="PaymentServiceExample_WithExceptionable"/> illustrates how
    /// using the <see cref="Exceptionable{T}"/> class can make this neater and easier to follow.
    /// </summary>
    public class PaymentServiceExample_WithExceptions
    {
        private readonly bool accountIdExists;
        private readonly bool isWebServiceDown;

        private readonly Log log;

        public PaymentServiceExample_WithExceptions(bool accountIdExists = true, bool isWebServiceDown = false)
        {
            this.accountIdExists = accountIdExists;
            this.isWebServiceDown = isWebServiceDown;

            this.log = new Log();
        }

        public PaymentResultEnum TakePayment(int accountId = 12345)
        {
            try
            {
                var account = this.GetAccount(accountId);

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

                //this.logger.LogError($"Payment failed.  Account ID = {accountId}, Name = {account.Name}.", ex);

                this.log.Error($"Payment failed.  Account ID = {accountId}, Name = Unknown.", ex);

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
            if (!this.accountIdExists)
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
            if (this.isWebServiceDown)
            {
                throw new WebException("The web request failed :(");
            }
            else
            {
                return "OK";
            }
        }
    }
}
using System;

namespace EAS.Exceptionable
{
    public enum PaymentResultEnum
    {
        Success,
        NoSuchAccount,
        WebServiceCallFailed
    }

    public class Log
    {
        public void Info(string message)
        {
            Console.WriteLine("INFO: " + message);
        }

        public void Error(string message, Exception ex)
        {
            Console.WriteLine("ERROR: " + message + "\r\n" + ex);
        }
    }

    public class AccountRepository
    {
        public void SaveAccount(Account account)
        {
            Console.WriteLine($"Saving the account: ID = {account.Id}, Name = \"{account.Name}\"");
        }
    }

    public class Account
    {
        public Account(int id, string name)
        {
            this.Id = id;
            this.Name = name;
        }

        public int Id { get; }
        public string Name { get; }
    }
}
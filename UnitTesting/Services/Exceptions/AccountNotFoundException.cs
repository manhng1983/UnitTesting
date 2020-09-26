using System;

namespace UnitTestArticle.Services.Exceptions
{
    public class AccountNotFoundException : Exception
    {
        public string AccountNumber { get; private set; }
        public int ID { get; private set; }

        public AccountNotFoundException(int id)
        {
            this.ID = id;
        }

        public AccountNotFoundException(string accountNumber)
        {
            this.AccountNumber = accountNumber;
        }
    }
}
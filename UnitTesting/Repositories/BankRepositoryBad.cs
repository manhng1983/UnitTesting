using System;
using System.Linq;
using UnitTestArticle.Services.Exceptions;

namespace UnitTestArticle.Repositories
{
    public class BankRepositoryBad : IDisposable
    {
        private BankContext context;
        private bool disposed = false;

        public BankRepositoryBad(BankContext context)
        {
            this.context = context;
        }

        public BankRepositoryBad()
            : this (new BankContext())
        {

        }

        public Account GetAccount(string accountNumber)
        {
            return this.context
                .Accounts
                .FirstOrDefault(a => a.Account_Number == accountNumber.Trim());
        }

        public Account GetAccount(int id)
        {
            return this.context
                .Accounts
                .FirstOrDefault(a => a.ID == id);
        }

        public void SetBalance(int id, decimal balance)
        {
            Account account = GetAccount(id);
            if (account == null)
            {
                throw new AccountNotFoundException(id);
            }

            account.Balance = balance;

            context.SaveChanges();
        }

        public void AddTransaction(int id, decimal amount, decimal newBalance)
        {
            context.Transactions.Add(new Transaction
            {
                Account_ID = id,
                Amount = amount,
                New_Balance = newBalance,
                Transaction_Date = DateTime.Now
            });

            context.SaveChanges();
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.context.Dispose();
                }
            }
            this.disposed = true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
    }
}
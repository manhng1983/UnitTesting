using System;
using System.Linq;
using UnitTestArticle.Interfaces;

namespace UnitTestArticle.Repositories
{
    public class BankRepository : IBankRepository
    {
        private BankContext context;
        private bool disposed = false;

        public BankRepository(BankContext context)
        {
            this.context = context;
        }

        public BankRepository()
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
                throw new ApplicationException("Account not found");
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
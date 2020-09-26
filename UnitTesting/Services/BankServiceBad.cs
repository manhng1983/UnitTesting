using System;
using UnitTestArticle.Repositories;
using UnitTestArticle.Services.Exceptions;

namespace UnitTestArticle.Services
{
    public class BankServiceBad : IDisposable
    {
        private BankRepositoryBad repo;
        private bool disposed = false;

        public BankServiceBad()
        {
            repo = new BankRepositoryBad();
        }

        public Account GetAccount(string accountNumber)
        {
            return repo.GetAccount(accountNumber);
        }

        public void UpdateAccountBalance(Account account, decimal amount)
        {
            if (account == null)
            {
                throw new ArgumentNullException(nameof(account));
            }

            repo.SetBalance(account.ID, account.Balance += amount);

            repo.AddTransaction(account.ID, amount, account.Balance);

            if (account.Balance < 0)
            {
                var reportingService = new ReportingService();

                reportingService.AccountIsOverdrawn(account.ID);
            }
        }

        public void TransferMoney(string sourceAccountNumber, string destinationAccountNumber, decimal transferAmount)
        {
            if (transferAmount <= 0)
            {
                throw new InvalidAmountException();
            }

            Account sourceAccount = repo.GetAccount(sourceAccountNumber);

            if (sourceAccount == null)
            {
                throw new AccountNotFoundException(sourceAccountNumber);
            }

            Account destinationAccount = repo.GetAccount(destinationAccountNumber);

            if (destinationAccount == null)
            {
                throw new AccountNotFoundException(destinationAccountNumber);
            }

            if (sourceAccount.Balance < transferAmount)
            {
                throw new InsufficientFundsException();
            }

            // remove transferAmount from destination account
            repo.SetBalance(sourceAccount.ID, sourceAccount.Balance -= transferAmount);

            // record the transaction
            repo.AddTransaction(sourceAccount.ID, -transferAmount, sourceAccount.Balance);

            // add transferAmount to source account
            repo.SetBalance(destinationAccount.ID, destinationAccount.Balance += transferAmount);

            // record the transaction
            repo.AddTransaction(destinationAccount.ID, transferAmount, destinationAccount.Balance);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposed)
            {
                if (disposing)
                {
                    this.repo.Dispose();
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
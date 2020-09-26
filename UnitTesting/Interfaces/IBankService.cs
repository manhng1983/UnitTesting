using System;

namespace UnitTestArticle.Interfaces
{
    public interface IBankService : IDisposable
    {
        Account GetAccount(string accountNumber);
        void TransferMoney(string sourceAccountNumber, string destinationAccountNumber, decimal transferAmount);
        void UpdateAccountBalance(Account account, decimal amount);
    }
}
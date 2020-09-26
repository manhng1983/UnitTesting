using System;

namespace UnitTestArticle.Interfaces
{
    public interface IBankRepository : IDisposable
    {
        Account GetAccount(string accountNumber);
        Account GetAccount(int id);
        void SetBalance(int id, decimal balance);
        void AddTransaction(int id, decimal amount, decimal newBalance);
    }
}

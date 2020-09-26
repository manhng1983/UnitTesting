using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using UnitTestArticle.Interfaces;
using UnitTestArticle.Services;
using UnitTestArticle.Services.Exceptions;

namespace UnitTestArticle.Tests.Services
{
    [TestClass]
    [Ignore]
    public class BankServiceTestsSelfShunt : IBankRepository // our test class implements IBankRepository
    {
        #region Self-shunt

        // Our test class is going to implement a mocked version of IBankRepository
        // using Lists to hold the data rather than database tables.  The code in this
        // region is a List based implementation of the IBankRepository methods

        private List<Account> Accounts;
        private List<Transaction> Transactions;
        private int GetAccountCalled;

        Account IBankRepository.GetAccount(string accountNumber)
        {
            GetAccountCalled++;

            return Accounts.FirstOrDefault(a => a.Account_Number == accountNumber);
        }

        Account IBankRepository.GetAccount(int id)
        {
            return Accounts.FirstOrDefault(a => a.ID == id);
        }

        void IBankRepository.SetBalance(int id, decimal balance)
        {
            Accounts.FirstOrDefault(a => a.ID == id).Balance = balance;
        }

        void IBankRepository.AddTransaction(int id, decimal amount, decimal newBalance)
        {
            Transactions.Add(new Transaction { Account_ID = id, Amount = amount, New_Balance = newBalance });
        }

        void IDisposable.Dispose()
        {

        }

        #endregion

        private BankService bankService;

        [TestInitialize]
        public void Init()
        {
            // This is called at the start of every test

            Accounts = new List<Account>();
            Transactions = new List<Transaction>();
            GetAccountCalled = 0;

            // Add two test accounts to the Accounts collection
            Accounts.Add(new Account { ID = 1, Account_Number = "test1", Balance = 0 });
            Accounts.Add(new Account { ID = 2, Account_Number = "test2", Balance = 0 });

            bankService = new BankService(this, null);
        }

        [TestMethod]
        public void GetAccount_WithValidAccount_ReturnsAccount()
        {
            // Arrange

            // Act

            Account account = bankService.GetAccount("test1");

            // Assert

            Assert.IsNotNull(account);
            Assert.AreEqual(1, account.ID);
            Assert.AreEqual("test1", account.Account_Number);
        }

        [TestMethod]
        public void GetAccount_CallsRepo()
        {
            // Arrange

            // Act

            Account account = bankService.GetAccount("test1");

            // Assert

            Assert.IsTrue(GetAccountCalled > 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void TransferMoney_WithInsufficientFunds_RaisesException()
        {
            // Arrange

            bankService.GetAccount("test1").Balance = 50;

            // Act

            bankService.TransferMoney("test1", "test2", 100);

            // Assert
        }

        [TestMethod]
        public void TransferMoney_WithSufficientFunds_AccountsUpdated()
        {
            // Arrange

            bankService.GetAccount("test1").Balance = 50;

            // Act

            bankService.TransferMoney("test1", "test2", 10);

            // Assert

            Assert.AreEqual(40, ((IBankRepository)this).GetAccount("test1").Balance);
            Assert.AreEqual(10, ((IBankRepository)this).GetAccount("test2").Balance);
        }
    }
}

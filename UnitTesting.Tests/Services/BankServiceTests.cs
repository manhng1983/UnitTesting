using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using UnitTestArticle.Interfaces;
using UnitTestArticle.Services;
using UnitTestArticle.Services.Exceptions;

namespace UnitTestArticle.Tests.Services
{
    [TestClass]
    public class BankServiceTests
    {
        private BankService bankService;
        private Mock<IBankRepository> repoMock;
        private Mock<IReportingService> reportingMock;
        private Account test1Account;
        private Account test2Account;

        [TestInitialize]
        public void Init()
        {
            // This is called at the start of every test

            // Create two test accounts
            test1Account = new Account { ID = 1, Account_Number = "test1", Balance = 0 };
            test2Account = new Account { ID = 2, Account_Number = "test2", Balance = 0 };

            // Mock the classes we are abstracting
            repoMock = new Mock<IBankRepository>();
            reportingMock = new Mock<IReportingService>();

            // Ensure GetAccount returns the relevant Account object
            repoMock.Setup(m => m.GetAccount("test1")).Returns(test1Account);
            repoMock.Setup(m => m.GetAccount("test2")).Returns(test2Account);

            bankService = new BankService(repoMock.Object, reportingMock.Object);
        }

        [TestMethod]
        public void GetAccount_WithValidAccount_ReturnsAccount()
        {
            // Arrange

            // Act

            Account account = bankService.GetAccount("test1");

            // Assert

            Assert.IsNotNull(account);
            Assert.AreSame(test1Account, account);
        }

        [TestMethod]
        [ExpectedException(typeof(InsufficientFundsException))]
        public void TransferMoney_WithInsufficientFunds_RaisesException()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.TransferMoney("test1", "test2", 100);

            // Assert
        }

        [TestMethod]
        public void TransferMoney_WithSufficientFunds_SourceAccountUpdated()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.TransferMoney("test1", "test2", 10);

            // Assert

            repoMock.Verify(m => m.SetBalance(1, 40), Times.Once);
        }

        [TestMethod]
        public void TransferMoney_WithSufficientFunds_DestinationAccountUpdated()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.TransferMoney("test1", "test2", 10);

            // Assert

            repoMock.Verify(m => m.SetBalance(2, 10), Times.Once);
        }

        [TestMethod]
        public void TransferMoney_WithSufficientFunds_SourceTransactionCreated()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.TransferMoney("test1", "test2", 10);

            // Assert

            repoMock.Verify(m => m.AddTransaction(1, -10, It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod]
        public void TransferMoney_WithSufficientFunds_DestinationTransactionCreated()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.TransferMoney("test1", "test2", 10);

            // Assert

            repoMock.Verify(m => m.AddTransaction(2, 10, It.IsAny<decimal>()), Times.Once);
        }

        [TestMethod]
        public void UpdateAccountBalance_WithPositiveAmount_IncreasesBalance()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.UpdateAccountBalance(test1Account, 10);

            // Assert

            repoMock.Verify(m => m.SetBalance(1, 60), Times.Once);
        }

        [TestMethod]
        public void UpdateAccountBalance_WithNegativeAmount_DecreasesBalance()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.UpdateAccountBalance(test1Account, -10);

            // Assert

            repoMock.Verify(m => m.SetBalance(1, 40), Times.Once);
        }

        [TestMethod]
        public void UpdateAccountBalance_WithPositiveBalance_DoesNotReportOverdrawn()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.UpdateAccountBalance(test1Account, 10);

            // Assert

            reportingMock.Verify(m => m.AccountIsOverdrawn(1), Times.Never);
        }

        [TestMethod]
        public void UpdateAccountBalance_WithZeroBalance_DoesNotReportOverdrawn()
        {
            // Arrange

            test1Account.Balance = 0;

            // Act

            bankService.UpdateAccountBalance(test1Account, 0);

            // Assert

            reportingMock.Verify(m => m.AccountIsOverdrawn(1), Times.Never);
        }

        [TestMethod]
        public void UpdateAccountBalance_WithNegativeBalance_ReportsOverdrawn()
        {
            // Arrange

            test1Account.Balance = 10;

            // Act

            bankService.UpdateAccountBalance(test1Account, -20);

            // Assert

            reportingMock.Verify(m => m.AccountIsOverdrawn(1), Times.Once);
        }

        [TestMethod]
        public void UpdateAccountBalance_TransactionRecorded()
        {
            // Arrange

            test1Account.Balance = 50;

            // Act

            bankService.UpdateAccountBalance(test1Account, 10);

            // Assert

            repoMock.Verify(m => m.AddTransaction(1, 10, 60), Times.Once);
        }
    }
}

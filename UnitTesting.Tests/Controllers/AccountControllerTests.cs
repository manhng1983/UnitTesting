using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Web.Mvc;
using UnitTestArticle.Controllers;
using UnitTestArticle.Interfaces;
using UnitTestArticle.Models;
using UnitTestArticle.Services.Exceptions;

namespace UnitTestArticle.Tests.Controllers
{
    [TestClass]
    public class AccountControllerTests
    {
        private Mock<IBankService> bankServiceMock;
        private Mock<ISessionManager> sessionManagerMock;
        private Mock<IEmailService> emailService;

        [TestInitialize]
        public void Init()
        {
            bankServiceMock = new Mock<IBankService>();
            sessionManagerMock = new Mock<ISessionManager>();
            emailService = new Mock<IEmailService>();
        }

        [TestMethod]
        public void TransferMoney_HappyPath_TransfersMoney()
        {
            // Arrange

            var model = new TransferMoneyModel
            {
                SourceAccountNumber = "test1",
                DestinationAccountNumber = "test2",
                Amount = 123
            };

            var controller = new AccountController(bankServiceMock.Object, sessionManagerMock.Object, emailService.Object);

            // Act
            RedirectToRouteResult result = controller.TransferMoney(model) as RedirectToRouteResult;

            // Assert
            Assert.IsNotNull(result);

            // assert TransferMoney was called on the bank service
            bankServiceMock.Verify(m => m.TransferMoney("test1", "test2", 123), Times.Once);

            // assert the source account number was stored in the session
            sessionManagerMock.Verify(m => m.Store("SourceAccountNumber", "test1"));

            // assert the confirmation email was sent
            emailService.Verify(m => m.SendTransferEmailConfirmation("test1", "test2", 123), Times.Once);

            // assert the result is a redirect to Index
            Assert.AreSame("Index", result.RouteValues["action"]);
        }

        [TestMethod]
        public void TransferMoney_WithSameAccount_HasInvalidModelState()
        {
            // Arrange

            var model = new TransferMoneyModel
            {
                SourceAccountNumber = "test1",
                DestinationAccountNumber = "test1",
                Amount = 123
            };

            var controller = new AccountController(bankServiceMock.Object, sessionManagerMock.Object, emailService.Object);

            // Act
            ViewResult result = controller.TransferMoney(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count > 0);
            Assert.IsTrue(controller.ModelState.ContainsKey("SameAccount"));
        }

        [TestMethod]
        public void TransferMoney_WithInsufficientFunds_HasInvalidModelState()
        {
            // Arrange

            var model = new TransferMoneyModel
            {
                SourceAccountNumber = "test1",
                DestinationAccountNumber = "test2",
                Amount = 123
            };

            bankServiceMock.Setup(m => m.TransferMoney(model.SourceAccountNumber, model.DestinationAccountNumber, model.Amount))
                .Throws(new InsufficientFundsException());

            var controller = new AccountController(bankServiceMock.Object, sessionManagerMock.Object, emailService.Object);

            // Act
            ViewResult result = controller.TransferMoney(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count > 0);
            Assert.IsTrue(controller.ModelState.ContainsKey("InsufficientFunds"));
        }

        [TestMethod]
        public void TransferMoney_WithInvalidAccount_HasInvalidModelState()
        {
            // Arrange

            var model = new TransferMoneyModel
            {
                SourceAccountNumber = "test1",
                DestinationAccountNumber = "test2",
                Amount = 123
            };

            bankServiceMock.Setup(m => m.TransferMoney(model.SourceAccountNumber, model.DestinationAccountNumber, model.Amount))
                .Throws(new AccountNotFoundException("test1"));

            var controller = new AccountController(bankServiceMock.Object, sessionManagerMock.Object, emailService.Object);

            // Act
            ViewResult result = controller.TransferMoney(model) as ViewResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.IsTrue(controller.ModelState.Count > 0);
            Assert.IsTrue(controller.ModelState.ContainsKey("AccountNotFound"));
        }
    }
}

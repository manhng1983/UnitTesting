using System.Web.Mvc;
using UnitTestArticle.Interfaces;
using UnitTestArticle.Models;
using UnitTestArticle.Services;
using UnitTestArticle.Services.Exceptions;

namespace UnitTestArticle.Controllers
{
    public class AccountController : Controller
    {
        private IBankService bankService;
        private ISessionManager sessionManager;
        private IEmailService emailService;

        public AccountController()
            : this (new BankService(), new SessionManager(), new EmailService())
        {

        }

        public AccountController(IBankService bankService, ISessionManager sessionManager, IEmailService emailService)
        {
            this.bankService = bankService;
            this.sessionManager = sessionManager;
            this.emailService = emailService;
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult TransferMoney()
        {
            string defaultAccount = this.sessionManager.Get<string>("SourceAccountNumber");

            return View();
        }

        [HttpPost]
        public ActionResult TransferMoney(TransferMoneyModel model)
        {
            if (model.SourceAccountNumber == model.DestinationAccountNumber)
            {
                ModelState.AddModelError("SameAccount", "The source and destination accounts must be different");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                bankService.TransferMoney(model.SourceAccountNumber, model.DestinationAccountNumber, model.Amount);
            }
            catch (InsufficientFundsException)
            {
                ModelState.AddModelError("InsufficientFunds", "There were insufficient funds to complete the transfer.");
            }
            catch (AccountNotFoundException ex)
            {
                ModelState.AddModelError("AccountNotFound", $"There was a problem finding account {ex.AccountNumber}.");
            }
            catch
            {
                ModelState.AddModelError("TransferFailed", "There was a problem with the transfer, please contact your bank.");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            this.sessionManager.Store("SourceAccountNumber", model.SourceAccountNumber);

            this.emailService.SendTransferEmailConfirmation(model.SourceAccountNumber, model.DestinationAccountNumber, model.Amount);

            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bankService.Dispose();
            }

            base.Dispose(disposing);
        }
    }
}
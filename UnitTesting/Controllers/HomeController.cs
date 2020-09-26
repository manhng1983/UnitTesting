using System.Web.Mvc;
using UnitTestArticle.Services;
using UnitTestArticle.Services.Exceptions;

namespace UnitTestArticle.Controllers
{
    public class HomeController : Controller
    {
        private BankService bankService;

        public HomeController()
        {
            bankService = new BankService();
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                bankService.Dispose();
            }

            base.Dispose(disposing);
        }

        public ActionResult Index()
        {

            using (var bankService = new BankServiceBad())
            {
                string accountNumber = "1111111111";
                var account = bankService.GetAccount(accountNumber);
                if (account == null)
                {
                    throw new AccountNotFoundException(accountNumber);
                }

                // Add 100 to account 1111111111
                bankService.UpdateAccountBalance(account, 100);
            }

            return View();
            
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
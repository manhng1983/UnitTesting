using UnitTestArticle.Interfaces;

namespace UnitTestArticle.Services
{
    public class EmailService : IEmailService
    {
        public void SendTransferEmailConfirmation(string sourceAccountNumber, string destinationAccountNumber, decimal transferAmount)
        {
            // code to send email confirmation here
        }
    }
}
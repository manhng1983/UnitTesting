namespace UnitTestArticle.Interfaces
{
    public interface IEmailService
    {
        void SendTransferEmailConfirmation(string sourceAccountNumber, string destinationAccountNumber, decimal transferAmount);
    }
}

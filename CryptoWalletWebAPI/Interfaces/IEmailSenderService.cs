using CryptoWalletWebAPI.Models;

namespace CryptoWalletWebAPI.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string subject, string htmlMessage, string registeredEmail);

        public string EmailFormat(CryptoUser user);

        public string EmailFormat(SpecificUser specificUser, Transaction transaction, string transactionType);
    }
}

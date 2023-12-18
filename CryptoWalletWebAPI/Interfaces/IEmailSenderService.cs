namespace CryptoWalletWebAPI.Interfaces
{
    public interface IEmailSenderService
    {
        Task SendEmailAsync(string subject, string htmlMessage, string registeredEmail);
    }
}

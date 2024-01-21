using CryptoWalletWebAPI.Models;
using CryptoWalletWebAPI.Interfaces;
using Microsoft.Extensions.Options;
//using System.Net.Mail;


using Microsoft.Extensions.Options;
using MimeKit;
using System.Net;
using MailKit.Net.Smtp;

namespace CryptoWalletWebAPI.Services
{
    public class EmailSenderService : IEmailSenderService
    {

        private readonly EmailSettings emailSettings;


        public EmailSenderService(IOptions<EmailSettings> emailSettings)
        {
            this.emailSettings = emailSettings.Value;
        }


        public async Task SendEmailAsync(string subject, string htmlMessage, string email)
        {
            var mimeMessage = new MimeMessage(); 
            mimeMessage.From.Add(new MailboxAddress(this.emailSettings.SenderName, this.emailSettings.SenderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(email));
            mimeMessage.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlMessage };

            mimeMessage.Body = builder.ToMessageBody();

            try
            {
                var client = new SmtpClient();
                client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                await client.ConnectAsync(this.emailSettings.MailServer, this.emailSettings.MailPort, this.emailSettings.UseSsl)
                    .ConfigureAwait(false);

                await client.AuthenticateAsync(this.emailSettings.SenderUserName, this.emailSettings.Password).ConfigureAwait(false);
                await client.SendAsync(mimeMessage).ConfigureAwait(false);
                await client.DisconnectAsync(true).ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.ToString());
            }

        }

        public string EmailFormat(CryptoUser user)
        {
            var htmlMessage = $@"
                <p>Welcome to CryptoWallets <strong>{user.FirstName} {user.LastName}!!</strong></p>
                <br>
                <p>Private Key: <strong>{user.PrivateKey}</strong></p>
                <p>Total Balance: <strong>{user.TotalBalance}</strong></p>
                <br>
                <p>Please enjoy your stay</p>
                ";

            return htmlMessage;
        }

        public string EmailFormat(SpecificUser specificUser, Transaction transaction, string transactionType)
        {
            var htmlMessage = $@"
                <p>{transactionType}: <strong>{transaction.Amount}!!</strong></p>
                <br>
                <p>Remaining Balance: <strong>{specificUser.Balance}</strong></p>
                <br>
                <p>Penjoy your stay</p>
                ";

            return htmlMessage;
        }
    }
}

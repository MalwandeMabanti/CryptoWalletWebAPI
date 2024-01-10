using CryptoWalletWebAPI.Interfaces;
using CryptoWalletWebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Org.BouncyCastle.Tls;
using System.Net.NetworkInformation;
using System.Security.Claims;
using static System.Net.Mime.MediaTypeNames;

using System.Text.Json;
using Microsoft.AspNetCore.Http.HttpResults;

namespace CryptoWalletWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService walletService;
        private readonly IEmailSenderService emailSenderService;

        public WalletController(IWalletService walletService, IEmailSenderService emailSenderService)
        {
            this.walletService = walletService;
            this.emailSenderService = emailSenderService;
        }

        [Authorize]
        [HttpPost("send")]
        public async Task<IActionResult> AddTransaction(Transaction transaction) 
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }

            var sendingUser = await this.walletService.GetReceivingUser(User.FindFirstValue(ClaimTypes.Email));
            var receivingUser = await this.walletService.GetReceivingUser(transaction.RecipientEmail);

            if (receivingUser == null)
            {
                return this.NotFound("This use does not have an account");
            };

            await this.walletService.AddTransaction(sendingUser, receivingUser, transaction);

            await this.emailSenderService.SendEmailAsync("Money Sent", this.EmailFormat(sendingUser, transaction, "Money sent"), sendingUser.Email);

            await this.emailSenderService.SendEmailAsync("Money Received", this.EmailFormat(receivingUser, transaction, "Money received"), receivingUser.Email);

            return Ok();
        }

        [Authorize]
        [HttpGet("GetSpecificUserWithTransactions")]
        public async Task<IActionResult> GetSpecificUserWithTransactions()
        {
            var specificUser = await this.walletService.GetSpecificUserWithTransactions(User.FindFirstValue(ClaimTypes.Email));

            if (specificUser == null) 
            {
                return this.NotFound("User not found");
            }

            return Ok(specificUser);
        }

        [HttpGet("GetAllUserTransactions")]
        public async Task<IActionResult> GetAllUserTransactions()
        {
            return Ok(await this.walletService.GetAllTransactionsAsync());
        }

        private string EmailFormat(SpecificUser specificUser, Transaction transaction, string transactionType)
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

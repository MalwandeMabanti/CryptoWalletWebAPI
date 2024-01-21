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
using FluentValidation;
using CryptoWalletWebAPI.Validators;

namespace CryptoWalletWebAPI.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService walletService;
        private readonly IEmailSenderService emailSenderService;
        private readonly IValidator<Transaction> validator;

        public WalletController(
            IWalletService walletService, 
            IEmailSenderService emailSenderService,
            IValidator<Transaction> validator)
        {
            this.walletService = walletService;
            this.emailSenderService = emailSenderService;
            this.validator = validator;
        }

        [HttpPost("send")]
        public async Task<IActionResult> AddTransaction(Transaction transaction) 
        {

            var result = this.validator.Validate(transaction, _ => _.IncludeRuleSets("Create"));

            if (!result.IsValid) 
            {
                result.AddModelState(this.ModelState);

                return this.BadRequest(this.ModelState);
            }

            var sendingUser = await this.walletService.GetReceivingUser(User.FindFirstValue(ClaimTypes.Email));
            var receivingUser = await this.walletService.GetReceivingUser(transaction.RecipientEmail);

            if (transaction.UserId != sendingUser.UserId) 
            {
                return BadRequest($"The private key '{transaction.UserId}' does not match '{sendingUser.UserId}'");
            }

            if (receivingUser == null)
            {
                return this.NotFound("This use does not have an account");
            };

            await this.walletService.AddTransaction(sendingUser, receivingUser, transaction);

            await this.emailSenderService.SendEmailAsync("Money Sent", this.emailSenderService.EmailFormat(sendingUser, transaction, "Money sent"), sendingUser.Email);

            await this.emailSenderService.SendEmailAsync("Money Received", this.emailSenderService.EmailFormat(receivingUser, transaction, "Money received"), receivingUser.Email);

            return Ok();
        }

        [HttpGet("GetSpecificUserWithOutTransactions")]
        public async Task<IActionResult> GetSpecificUserWithOutTransactions()
        {
            var specificUser = await this.walletService.GetSpecificUserWithOutTransactions(User.FindFirstValue(ClaimTypes.Email));

            if (specificUser == null) 
            {
                return this.NotFound("User not found");
            }

            return Ok(specificUser);
        }

        [HttpGet("GetSpecificUserWithInTransactions")]
        public async Task<IActionResult> GetSpecificUserWithInTransactions()
        {
            var specificUser = await this.walletService.GetSpecificUserWithInTransactions(User.FindFirstValue(ClaimTypes.Email));

            if (specificUser == null)                  
            {
                return this.NotFound("User not found");
            }

            return Ok(specificUser);
        }
    }


}

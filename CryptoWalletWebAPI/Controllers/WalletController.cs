using CryptoWalletWebAPI.Interfaces;
using CryptoWalletWebAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService walletService;

        public WalletController(IWalletService walletService) 
        { 
            this.walletService = walletService;
        }

        [Authorize]
        [HttpPost("send")]
        public IActionResult Send(Transaction model) 
        {
            if (!ModelState.IsValid) 
            { 
                return BadRequest(ModelState);
            }

            var transaction = new Transaction 
            { 
                RecipientEmail = model.RecipientEmail,
                Amount = model.Amount
            };

            this.walletService.AddTransaction(transaction);

            return Ok();
        }
    }
}

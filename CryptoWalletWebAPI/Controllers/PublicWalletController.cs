using CryptoWalletWebAPI.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CryptoWalletWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublicWalletController : ControllerBase
    {
        private readonly IWalletService walletService;

        public PublicWalletController(IWalletService walletService)
        {
            this.walletService = walletService;
        }

        [HttpGet("GetAllUserTransactions")]
        public async Task<IActionResult> GetAllUserTransactions()
        {
            return Ok(await this.walletService.GetAllTransactionsAsync());
        }
    }
}

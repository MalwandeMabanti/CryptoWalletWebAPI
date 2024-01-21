using CryptoWalletWebAPI.Interfaces;
using CryptoWalletWebAPI.Models;
using CryptoWalletWebAPI.Validators;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace CryptoWalletWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<CryptoUser> _userManager;
        private readonly SignInManager<CryptoUser> _signInManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailSenderService emailSenderService;
        private readonly IWalletService walletService;
        private readonly IAuthService authService;
        private readonly IValidator<Register> validator;

        public AuthController(UserManager<CryptoUser> userManager,
            SignInManager<CryptoUser> signInManager,
            IConfiguration configuration,
            IEmailSenderService emailSenderService,
            IWalletService walletService,
            IAuthService authService,
            IValidator<Register> validator)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.emailSenderService = emailSenderService;   
            this.walletService = walletService;
            this.authService = authService;
            this.validator = validator;
        }

        // POST api/auth/login
        [HttpPost("Login")]
        public async Task<IActionResult> Login([FromBody] Login model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(this.ModelState);
            }

            var user = await _userManager.FindByNameAsync(model.Email);
            if (user == null && await _userManager.CheckPasswordAsync(user, model.Password))
            {
                return Unauthorized("You are not a registered user.");
            }

            return Ok(new { Token = this.authService.GenerateJsonWebToken(user), UserName = user.Email });
        }

        // POST api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
            var registerResult = this.validator.Validate(model, _ => _.IncludeRuleSets("Create"));

            if (!registerResult.IsValid) 
            {
                registerResult.AddModelState(this.ModelState);

                return this.BadRequest(this.ModelState);
            }

            var existingUser = await this._userManager.FindByEmailAsync(model.Email);

            if (existingUser != null)
            {
                return this.BadRequest("User exists for this email");
            }

            var user = new CryptoUser
            {
                UserName = model.Email,
                Email = model.Email,
                FirstName = model.FirstName,
                LastName = model.LastName,
                PrivateKey = this.authService.GeneratePrivateKey(),
                TotalBalance = 60000
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                return BadRequest("There was a problem creating the user");
            }

            var newUser = new SpecificUser
            {
                UserId = user.PrivateKey,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Balance = 60000

            };

            await this.walletService.AddUser(newUser);

            await this.emailSenderService.SendEmailAsync("Crypto Wallet Registration", this.emailSenderService.EmailFormat(user), user.Email);

            return Ok(new { token = this.authService.GenerateJsonWebToken(user), data = "User registered successfully! You can now login" });
        }
    }
}

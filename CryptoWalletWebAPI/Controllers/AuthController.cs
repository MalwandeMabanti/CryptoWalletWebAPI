using CryptoWalletWebAPI.Interfaces;
using CryptoWalletWebAPI.Models;
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

        public AuthController(UserManager<CryptoUser> userManager,
            SignInManager<CryptoUser> signInManager,
            IConfiguration configuration,
            IEmailSenderService emailSenderService,
           
            IWalletService walletService)

        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
            this.emailSenderService = emailSenderService;   
            this.walletService = walletService;
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

            return Ok(new { Token = GenerateJsonWebToken(user), UserName = user.Email });
        }

        // POST api/auth/register
        [HttpPost("Register")]
        public async Task<IActionResult> Register([FromBody] Register model)
        {
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
                PrivateKey = this.GeneratePrivateKey(),
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

            await this.emailSenderService.SendEmailAsync("Crypto Wallet Registration", this.EmailFormat(user), user.Email);

            return Ok(new { token = GenerateJsonWebToken(user), data = "User registered successfully! You can now login" });
        }

        private string GeneratePrivateKey()
        {
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                byte[] randomBytes = new byte[8]; 
                rng.GetBytes(randomBytes);
                return BitConverter.ToString(randomBytes).Replace("-", string.Empty);
            }
        }

        private string EmailFormat(CryptoUser user)
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

        private string GenerateJsonWebToken(CryptoUser user)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] {
            new Claim(ClaimTypes.NameIdentifier, user.PrivateKey),
            new Claim(ClaimTypes.Email, user.Email),
            new Claim(ClaimTypes.GivenName, user.FirstName + " " + user.LastName),
        };
            
            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                _configuration["Jwt:Issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}

using CryptoWalletWebAPI.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CryptoWalletWebAPI.Data
{
    public class AuthDbContext : IdentityDbContext<CryptoUser>
    {
        public AuthDbContext(DbContextOptions<AuthDbContext> options) 
            : base(options) 
        { }
    }
}

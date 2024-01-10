using Microsoft.AspNetCore.Identity;

namespace CryptoWalletWebAPI.Models
{
    public class CryptoUser : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PrivateKey { get; set; }
        public int TotalBalance { get; set; }

    }
}

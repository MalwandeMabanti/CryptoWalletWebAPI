using System.ComponentModel.DataAnnotations;

namespace CryptoWalletWebAPI.Models
{
    public class SpecificUser
    {
        [Key]
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? Email { get; set; }
        public int Balance { get; set; }
        public virtual ICollection<Transaction>? Transactions { get; set; }
    }
}

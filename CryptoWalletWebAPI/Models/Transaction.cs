using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletWebAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? SendingEmail { get; set; }
        public string? RecipientEmail { get; set; }
        public int Amount { get; set; }

        public string? TransactionType { get; set; }
        public string? UserId { get; set; }
        public virtual SpecificUser? SpecificUserDetails { get; set; }

    }
}

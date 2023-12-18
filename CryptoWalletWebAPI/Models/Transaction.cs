using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletWebAPI.Models
{
    public class Transaction
    {
        [Key]
        public int Id { get; set; }
        
        public string RecipientEmail { get; set; }

        public int Amount { get; set; }

    }
}

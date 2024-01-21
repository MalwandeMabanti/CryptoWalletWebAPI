
namespace CryptoWalletWebAPI.Models
{
    public class SpecificUserDto
    {
        public string UserId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public int Balance { get; set; }
        public List<TransactionDto> Transactions { get; set; }
    }
}

namespace CryptoWalletWebAPI.Models
{
    public class TransactionDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string SendingEmail { get; set; }
        public string RecipientEmail { get; set; }
        public int Amount { get; set; }
        public string UserId { get; set; }
    }
}

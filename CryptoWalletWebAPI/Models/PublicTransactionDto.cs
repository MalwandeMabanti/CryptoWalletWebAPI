namespace CryptoWalletWebAPI.Models
{
    public class PublicTransactionDto
    {
        public string SendingEmail { get; set; }
        public string RecipientEmail { get; set; }
        public int Amount { get; set; }
    }
}

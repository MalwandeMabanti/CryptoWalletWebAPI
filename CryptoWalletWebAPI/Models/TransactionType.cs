namespace CryptoWalletWebAPI.Models
{
    public class TransactionType
    {
        public int Id { get; set; }
        public string Type { get; set; }

        // Navigation property to Transactions
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}

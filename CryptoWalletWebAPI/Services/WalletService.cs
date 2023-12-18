using CryptoWalletWebAPI.Data;
using CryptoWalletWebAPI.Interfaces;
using CryptoWalletWebAPI.Models;

namespace CryptoWalletWebAPI.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext context;

        public WalletService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public void AddTransaction(Transaction transaction)
        {
            this.context.Transactions.Add(transaction);
            this.context.SaveChanges();
        }
    }
}

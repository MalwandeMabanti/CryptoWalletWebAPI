using CryptoWalletWebAPI.Models;

namespace CryptoWalletWebAPI.Interfaces
{
    public interface IWalletService
    {
        void AddTransaction(Transaction transaction);
    }
}

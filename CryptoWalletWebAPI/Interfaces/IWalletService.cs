using CryptoWalletWebAPI.Models;

namespace CryptoWalletWebAPI.Interfaces
{
    public interface IWalletService
    {
        Task<SpecificUser> GetReceivingUser(string receivingEmail);

        Task<SpecificUserDto> GetSpecificUserWithOutTransactions(string userId);

        Task<SpecificUserDto> GetSpecificUserWithInTransactions(string userId);

        Task AddUser(SpecificUser user);

        Task AddTransaction(SpecificUser sendingUser, SpecificUser receivingUser, Transaction transaction);

        Task<List<Transaction>> GetAllTransactionsAsync();
    }
}

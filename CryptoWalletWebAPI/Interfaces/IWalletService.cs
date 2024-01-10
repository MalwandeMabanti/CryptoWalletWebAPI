using CryptoWalletWebAPI.Models;

namespace CryptoWalletWebAPI.Interfaces
{
    public interface IWalletService
    {
        Task<SpecificUser> GetReceivingUser(string receivingEmail);
        Task<Transaction> GetDetailsByEmail(string email);

        Task<SpecificUserDto> GetSpecificUserWithTransactions(string userId);

        Task AddUser(SpecificUser user);

        Task<Transaction>? GetById(int id);

        Task AddTransaction(SpecificUser sendingUser, SpecificUser receivingUser, Transaction transaction);

        Task<List<Transaction>> GetAllTransactionsAsync();

        void DeleteTransaction(Transaction transaction);
        
    }
}

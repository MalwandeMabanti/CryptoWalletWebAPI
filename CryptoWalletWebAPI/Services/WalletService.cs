using CryptoWalletWebAPI.Data;
using CryptoWalletWebAPI.Interfaces;
using CryptoWalletWebAPI.Models;
using Microsoft.AspNet.Identity;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
//using System.Transactions;

namespace CryptoWalletWebAPI.Services
{
    public class WalletService : IWalletService
    {
        private readonly ApplicationDbContext context;

        public WalletService(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<SpecificUser> GetReceivingUser(string receivingEmail) 
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await this.context.SpecificUsers
                .Where(_ => _.Email == receivingEmail)
                .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<Transaction> GetDetailsByEmail(string email) 
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await this.context.Transactions
                .Where(_ => _.SendingEmail == email)
                .FirstOrDefaultAsync();
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<SpecificUserDto> GetSpecificUserWithTransactions(string email)
        {
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8604 // Possible null reference argument.
            return await this.context.SpecificUsers
                .Where(_ => _.Email == email)
                .Select(_ => new SpecificUserDto
                {
                    UserId = _.UserId,
                    FirstName = _.FirstName,
                    LastName = _.LastName,
                    Email = _.Email,
                    Balance = _.Balance,
                    
                    Transactions = _.Transactions.Select(_ => new TransactionDto
                    {
                        // Map properties of Transaction except the one you want to exclude
                        Id = _.Id,
                        FirstName = _.FirstName,
                        LastName = _.LastName,
                        SendingEmail = _.SendingEmail,
                        RecipientEmail = _.RecipientEmail,
                        Amount = _.Amount,
                        UserId = _.UserId
                    }).ToList()
                })
                .FirstOrDefaultAsync();
#pragma warning restore CS8604 // Possible null reference argument.
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task<Transaction>? GetById(int id)
        {
#pragma warning disable CS8603 // Possible null reference return.
            return await this.context.Transactions.FindAsync(id);
#pragma warning restore CS8603 // Possible null reference return.
        }

        public async Task AddUser(SpecificUser user)
        {
            await this.context.SpecificUsers.AddAsync(user);
            this.context.SaveChanges();
        }

        private Transaction CreateTransaction(Transaction transaction, SpecificUser sendingUser) 
        {
            return new Transaction 
            { 
                FirstName = sendingUser.FirstName, 
                LastName = sendingUser.LastName,
                SendingEmail = sendingUser.Email,
                RecipientEmail = transaction.RecipientEmail,
                UserId = sendingUser.UserId,
                Amount = transaction.Amount,
            };
        }

        private SpecificUser UpdateSendingUserBalance(SpecificUser user, Transaction transaction) 
        {
            user.Balance = user.Balance - transaction.Amount;

            return user;
        }

        private SpecificUser UpdateReceivingUserBalance(SpecificUser user, Transaction transaction)
        {
            user.Balance = user.Balance + transaction.Amount;

            return user;
        }

        public async Task AddTransaction(SpecificUser sendingUser, SpecificUser receivingUser, Transaction currentTransaction)
        {
            using var transaction = this.context.Database.BeginTransaction();
            try
            {
                var createdTransaction = CreateTransaction(currentTransaction, sendingUser);

                await this.context.Transactions.AddAsync(createdTransaction);

                this.context.SaveChanges();

                var updateSendingUserBalance = UpdateSendingUserBalance(sendingUser, currentTransaction);

                this.context.SpecificUsers.Update(updateSendingUserBalance);

                this.context.SaveChanges();

                var updateReceivingUserBalance = UpdateReceivingUserBalance(receivingUser, currentTransaction);

                this.context.SpecificUsers.Update(updateReceivingUserBalance);

                this.context.SaveChanges();

                transaction.Commit();
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<List<Transaction>> GetAllTransactionsAsync()
        {
            return await this.context.Transactions.ToListAsync();
        }

        public void DeleteTransaction(Transaction transaction) 
        {
            this.context.Transactions.Remove(transaction);
            this.context.SaveChanges();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CryptoWalletWebAPI;
using CryptoWalletWebAPI.Models;

namespace CryptoWalletWebAPITests.Helpers
{
    public static class TransactionHelper
    {
        public static Transaction CreateTransaction() 
        { 
            return new Transaction() 
            {
                SendingEmail = "SendingEmail@g.com",
                RecipientEmail = "Receiving@g.com",
                Amount = 1222
            };
        }
    }
}

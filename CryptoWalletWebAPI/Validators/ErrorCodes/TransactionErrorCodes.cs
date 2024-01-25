using CryptoWalletWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletWebAPI.Validators.ErrorCodes
{
    public class TransactionErrorCodes
    {
        public const string RecipientEmail = "TE1";
        public const string Amount = "TE2";
    }
}

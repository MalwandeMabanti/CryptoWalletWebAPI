using CryptoWalletWebAPI.Models;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletWebAPI.Validators.ErrorCodes
{
    public class TransactionErrorCodes
    {
        public const string SendingEmail = "TE1";
        public const string RecipientEmail = "T2";
        public const string Amount = "TE3";
    }
}

using CryptoWalletWebAPI.Models;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletWebAPI.Validators.ErrorCodes
{
    public class RegisterErrorCodes : AbstractValidator<Register>
    {
        public const string FirstName = "AE1";
        public const string LastName = "AE2";
        public const string Email = "AE3";
        public const string Password = "AE4";
        public const string ConfirmPassword = "AE5";
    }
}

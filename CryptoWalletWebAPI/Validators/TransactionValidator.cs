using CryptoWalletWebAPI.Models;
using CryptoWalletWebAPI.Validators.ErrorCodes;
using FluentValidation;
using System.ComponentModel.DataAnnotations;

namespace CryptoWalletWebAPI.Validators
{
    public class TransactionValidator : AbstractValidator<Transaction>
    {
        public TransactionValidator() 
        {
            this.RuleSet("Create", () =>
            {
                this.GeneralRules();
            });
        }

        private void GeneralRules() 
        {
            this.RuleFor(_ => _.SendingEmail)
                .NotEmpty()
                .MaximumLength(15)
                .WithErrorCode(TransactionErrorCodes.SendingEmail);

            this.RuleFor(_ => _.RecipientEmail)
                .NotEmpty()
                .MaximumLength(15)
                .WithErrorCode(TransactionErrorCodes.RecipientEmail);

            this.RuleFor(_ => _.Amount)
                .NotEmpty()
                .GreaterThanOrEqualTo(0)
                .WithErrorCode(TransactionErrorCodes.Amount);
        } 
    }
}

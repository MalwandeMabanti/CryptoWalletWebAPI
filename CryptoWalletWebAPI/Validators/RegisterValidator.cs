using CryptoWalletWebAPI.Models;
using CryptoWalletWebAPI.Validators.ErrorCodes;
using FluentValidation;

namespace CryptoWalletWebAPI.Validators
{
    public class RegisterValidator : AbstractValidator<Register>
    {
        public RegisterValidator() 
        {
            RuleSet("Create", () =>
            {
                this.GeneralRules();
            });
        }

        private void GeneralRules() 
        {
            this.RuleFor(_ => _.FirstName)
                .NotEmpty()
                .MaximumLength(50)
                .WithErrorCode(RegisterErrorCodes.FirstName);

            this.RuleFor(_ => _.LastName)
                .NotEmpty()
                .MaximumLength(50)
                .WithErrorCode(RegisterErrorCodes.LastName);

            this.RuleFor(_ => _.Email)
                .NotEmpty()
                .WithErrorCode(RegisterErrorCodes.Email)
                .EmailAddress();

            this.RuleFor(_ => _.Password)
                .NotEmpty()
                .MinimumLength(6)
                .MaximumLength(15)
                .WithErrorCode(RegisterErrorCodes.Password);

            this.RuleFor(_ => _.ConfirmPassword)
                .NotEmpty()
                .Equal(_ => _.Password);
        }
    }

    
}

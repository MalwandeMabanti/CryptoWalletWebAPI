using CryptoWalletWebAPI.Models;
using CryptoWalletWebAPI.Validators;
using CryptoWalletWebAPITests.Helpers;
using FluentValidation.TestHelper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xunit;

namespace CryptoWalletWebAPITests
{
    public class TransactionValidatorTests
    {
        private readonly TransactionValidator validator;

        public TransactionValidatorTests()
        {
            this.validator = new TransactionValidator();
        }

        [Fact]
        public void CreateValidTransaction() 
        {
            var model = TransactionHelper.CreateTransaction();

            var result = this.validator.TestValidate(model, _ => _.IncludeRuleSets("Create"));

            Assert.True(result.IsValid, "qwer");
        }

        [Fact]
        public void CreateInvalidSendingEmail()
        {
            var model = TransactionHelper.CreateTransaction();

            model.SendingEmail = "qqqqqqqqwerrfgggggggggggggggggggggggggggr";

            var result = this.validator.TestValidate(model, _ => _.IncludeRuleSets("Create"));

            Assert.False(result.IsValid, "qwer");
        }

        [Fact]
        public void CreateInvalidReceivingEmail()
        {
            var model = TransactionHelper.CreateTransaction();

            model.RecipientEmail = "qqqqqqqqwerrfgggggggggggggggggggggggggggr";

            var result = this.validator.TestValidate(model, _ => _.IncludeRuleSets("Create"));

            Assert.False(result.IsValid, "qwer");
        }

        [Fact]
        public void CreateInvalidAmount() 
        {
            var model = TransactionHelper.CreateTransaction();

            model.Amount = -3455;

            var result = this.validator.TestValidate(model, _ => _.IncludeRuleSets("Create"));

            Assert.False(result.IsValid, "sddfdf");
        }

    }
}

using System;
using NUnit.Framework;
using PaymentGateway.API.Models.Validators;
using PaymentGateway.Common.Models.Payment;
using FluentAssertions;
using FluentValidation.TestHelper;

namespace PaymentGateway.API.Tests.Validator
{
    public class PaymentProcessingRequestValidatorTests
    {
        private protected PaymentProcessingRequestValidator _validator;
        
        [SetUp]
        public void Setup()
        {
            _validator = new PaymentProcessingRequestValidator();
        }

        private static double[] BadAmountTestSource = new double[]{0.0, Double.MaxValue};
        [TestCaseSource("BadAmountTestSource")]
        public  void GivenBadAmount_WhenIValidateMyRequest_ValidationErrorIsGenerated(double amount)
        {
            var requestModel = RequestModel();
            requestModel.Amount = amount;
            
            var result = _validator.TestValidate(requestModel);

            Assert.Multiple(()=>
            {
                result.IsValid.Should().BeFalse();
                result.ShouldHaveValidationErrorFor( x=> x.Amount);
            });
        }

        private static string[] BadCardNumbersTestSource = new string[]{string.Empty, null, "123456789012345678901234"};
        [TestCaseSource("BadCardNumbersTestSource")]
        public  void GivenBadCardNumber_WhenIValidateMyRequest_ValidationErrorIsGenerated(string cardNumber)
        {
            var requestModel = RequestModel();
            requestModel.CardNumber = cardNumber;
            
            var result = _validator.TestValidate(requestModel);

            Assert.Multiple(()=>
            {
                result.IsValid.Should().BeFalse();
                result.ShouldHaveValidationErrorFor( x=> x.CardNumber);
            });
        }

        private static int[] BadCVVTestSource = new int[]{0, Int32.MaxValue, Int32.MaxValue, -1, 1000};
        [TestCaseSource("BadCVVTestSource")]
        public  void GivenBadCVV_WhenIValidateMyRequest_ValidationErrorIsGenerated(int cvv)
        {
            var requestModel = RequestModel();
            requestModel.CVV = cvv;
            
            var result = _validator.TestValidate(requestModel);

            Assert.Multiple(()=>
            {
                result.IsValid.Should().BeFalse();
                result.ShouldHaveValidationErrorFor( x=> x.CVV);
            });
        }

        private static int[] BadExpiryYearTestSource = new int[]{0, Int32.MinValue, Int32.MaxValue, 2020, 3000};
        [TestCaseSource("BadExpiryYearTestSource")]
        public  void GivenBadExpiryYear_WhenIValidateMyRequest_ValidationErrorIsGenerated(int expiryYear)
        {
            var requestModel = RequestModel();
            requestModel.ExpiryYear = expiryYear;
            
            var result = _validator.TestValidate(requestModel);

            Assert.Multiple(()=>
            {
                result.IsValid.Should().BeFalse();
                result.ShouldHaveValidationErrorFor( x=> x.ExpiryYear);
            });
        }

        private static int[] BadExpiryMonthTestSource = new int[]{0, Int32.MinValue, Int32.MaxValue, 13, 14, -1};
        [TestCaseSource("BadExpiryMonthTestSource")]
        public  void GivenBadExpiryMonth_WhenIValidateMyRequest_ValidationErrorIsGenerated(int expiryMonth)
        {
            var requestModel = RequestModel();
            requestModel.ExpiryMonth = expiryMonth;
            
            var result = _validator.TestValidate(requestModel);

            Assert.Multiple(()=>
            {
                result.IsValid.Should().BeFalse();
                result.ShouldHaveValidationErrorFor( x=> x.ExpiryMonth);
            });
        }

        private static string[] BadCurrencyCodeTestSource = new string[]{string.Empty, "ABCDEFG", "AB", "ABCD"};
        [TestCaseSource("BadCurrencyCodeTestSource")]
        public  void GivenBadCurrencyCode_WhenIValidateMyRequest_ValidationErrorIsGenerated(string currencyCode)
        {
            var requestModel = RequestModel();
            requestModel.CurrencyCode = currencyCode;
            
            var result = _validator.TestValidate(requestModel);

            Assert.Multiple(()=>
            {
                result.IsValid.Should().BeFalse();
                result.ShouldHaveValidationErrorFor( x=> x.CurrencyCode);
            });
        }

        private PaymentProcessingRequest RequestModel()
        {
            return new PaymentProcessingRequest()
            {
                CurrencyCode = "GBP",
                ExpiryMonth = 1,
                ExpiryYear = 2021,
                CVV = 123,
                CardNumber = "12345657",
                Amount = 111
            };
        }        
    }
}
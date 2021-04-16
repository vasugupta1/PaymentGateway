using System;
using System.ComponentModel.DataAnnotations;

namespace PaymentGateway.Common.Models.Payment
{
    public class PaymentProcessingRequest
    {
        [Required]
        [StringLength(3, MinimumLength = 3, ErrorMessage = "Invalid currency code, please check your request")]
        public string CurrencyCode { get; set; } //ISO 4217 Codes are used
        [Required]
        [Range(1, 12, ErrorMessage = "Invalid expiry month value, please check your request")]
        public int ExpiryMonth { get; set; }
        [Required]
        [Range(2021, 2999, ErrorMessage = "Invalid expiry year value, please check your request")]
        public int ExpiryYear { get; set; }
        [Required]
        [Range(1, 999, ErrorMessage = "Invalid CVV Number, please check your request")]
        public int CVV { get; set; }
        [Required]
        [StringLength(19, MinimumLength = 1, ErrorMessage = "Invalid card number, please check your request")]
        public string CardNumber { get; set; }
        [Required]
        [Range(0.0, Double.MaxValue, ErrorMessage = "Invalid amount, please check your request")]
        public double Amount { get; set; }
    }
}
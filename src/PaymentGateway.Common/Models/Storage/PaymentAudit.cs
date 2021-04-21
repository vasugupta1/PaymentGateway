using PaymentGateway.Common.Models.Enums;
using System.Text.Json.Serialization;

namespace PaymentGateway.Common.Models.Storage
{
    public class PaymentAudit
    {
        [JsonPropertyName("currencyCode")]
        public string CurrencyCode { get; set; }
        [JsonPropertyName("expiryMonth")]
        public int ExpiryMonth { get; set; }
        [JsonPropertyName("expiryYear")]
        public int ExpiryYear { get; set; }
        [JsonPropertyName("cvv")]
        public int CVV { get; set; }
        [JsonPropertyName("cardNumber")]
        public string CardNumber { get; set; }
        [JsonPropertyName("amount")]
        public double Amount { get; set; }
        [JsonPropertyName("status")]
        public string Status { get; set; }
        [JsonPropertyName("transcationId")]
        public string TranscationId { get; set; }
    }
}
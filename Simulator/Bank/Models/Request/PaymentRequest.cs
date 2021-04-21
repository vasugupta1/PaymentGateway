namespace Bank.Models.Request
{
    public class PaymentRequest
    {
        public string CurrencyCode { get; set; } //ISO 4217 Codes are used
        
        public int ExpiryMonth { get; set; }
        
        public int ExpiryYear { get; set; }
        
        public int CVV { get; set; }
        
        public string CardNumber { get; set; }
        
        public double Amount { get; set; }
    }
}
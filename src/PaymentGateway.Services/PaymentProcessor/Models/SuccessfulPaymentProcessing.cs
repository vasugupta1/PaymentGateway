namespace PaymentGateway.Services.PaymentProcessor.Models
{
    public class SuccessfulPaymentProcessing
    {
        public string PaymentId { get; set; }
        public bool Success { get; set; }
        public string Message { get; set;}
    }
}
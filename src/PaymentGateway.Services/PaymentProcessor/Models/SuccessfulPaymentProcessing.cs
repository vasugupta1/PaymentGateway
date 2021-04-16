namespace PaymentGateway.Services.PaymentProcessor.Models
{
    public class SuccessfulPaymentProcessing
    {
        public string PaymentId { get; set; }
        public bool Success { get; set; } = true;
        public string Message { get; set;}
    }
}
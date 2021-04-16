namespace PaymentGateway.Services.PaymentProcessor.Models
{
    public class UnsuccessfulPaymentProcessing
    {
        public bool Success { get; set; } = false;
        public string Message { get; set; }
    }
}
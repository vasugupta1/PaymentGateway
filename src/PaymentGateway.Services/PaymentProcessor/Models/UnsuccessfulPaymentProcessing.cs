namespace PaymentGateway.Services.PaymentProcessor.Models
{
    public class UnsuccessfulPaymentProcessing
    {
        public bool Success { get; set; }
        public string Message { get; set; }
    }
}
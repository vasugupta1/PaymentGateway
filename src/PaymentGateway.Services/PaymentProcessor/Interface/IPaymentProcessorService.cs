using System.Threading.Tasks;
using OneOf;
using PaymentGateway.Common.Models.Payment;
using PaymentGateway.Services.PaymentProcessor.Models;

namespace PaymentGateway.Services.PaymentProcessor.Interface
{
    public interface IPaymentProcessorService
    {
        Task<OneOf<SuccessfulPaymentProcessing, UnsuccessfulPaymentProcessing>> ProcessPayment (PaymentProcessingRequest request);
    }
}